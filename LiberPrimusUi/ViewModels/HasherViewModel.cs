using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.Hash;
using LiberPrimusAnalysisTool.Database;
using LiberPrimusAnalysisTool.Entity.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiberPrimusUi.ViewModels;

public partial class HasherViewModel : ViewModelBase
{
    private readonly IMediator _mediator;

    public HasherViewModel(IMediator mediator)
    {
        _mediator = mediator;

        HashTypes.Add("SHA-512");
        HashTypes.Add("SHA3-512");
        HashTypes.Add("Blake2b");
        HashTypes.Add("Blake2s");
    }

    [ObservableProperty] private string _hashToMatch =
        "36367763ab73783c7af284446c59466b4cd653239a311cb7116d4618dee09a8425893dc7500b464fdaf1672d7bef5e891c6e2274568926a49fb4f45132c2a8b4";

    public ObservableCollection<string> HashTypes { get; set; } = new ObservableCollection<string>();

    [ObservableProperty] private string _selectedHash = "SHA-512";

    [ObservableProperty] private string _stringToHash = "";

    [ObservableProperty] private string _maxArrayLength = "512";

    [ObservableProperty] private string _result = "";

    [ObservableProperty] private bool _regenDataset = true;

    [ObservableProperty] private string _processed = "";

    private ConcurrentQueue<string> _tasks = new ConcurrentQueue<string>();
    
    private BigInteger _maxCombinations;
    
    private BigInteger _currentCombinations;
    
    private bool _keepGoing = true;

    [RelayCommand]
    public async void GenerateHash()
    {
        var command = new GenerateHashForByteArray.Command
        {
            ByteArray = System.Text.Encoding.UTF8.GetBytes(StringToHash),
            HashType = SelectedHash
        };

        Result = await _mediator.Send(command);
    }

    [RelayCommand]
    public async void HashingBruteForce()
    {
        _keepGoing = true;
        
        try
        {
            using (var context = new LiberContext())
            {
                await context.Database.EnsureCreatedAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        if (RegenDataset)
        {
            _ = Task.Run(() => GenerateAllByteArrays());
        }

        await Task.Delay(10000);

        Result += "Starting to hash byte arrays...\n";

        ulong counter = 0;
        ulong modulo = 1000;

        Random random = new Random();

        while (_keepGoing)
        {
            counter++;

            if (counter % modulo == 0)
            {
                modulo = (ulong)random.Next(1, 1000);
                Processed = $"Hashed {counter} items\n";
            }

            if (counter >= ulong.MaxValue - 100)
            {
                counter = 0;
            }

            using (var context = new LiberContext())
            {
                var item = await context.ProcessQueueItems.FirstOrDefaultAsync();

                if (item != null)
                {
                    List<byte> maxByteArray = item.HopperString.Split(",").Select(byte.Parse).ToList();

                    await Parallel.ForAsync(0, maxByteArray.Count, async (i, cancellationToken) =>
                    {
                        byte[] byteArray = new byte[i + 1];
                        maxByteArray.CopyTo(0, byteArray, 0, i + 1);
                        
                        foreach (var hashType in HashTypes)
                        {
                            var command = new GenerateHashForByteArray.Command
                            {
                                ByteArray = byteArray.ToArray(),
                                HashType = hashType
                            };

                            var hashResult = await _mediator.Send(command);

                            if (hashResult == HashToMatch)
                            {
                                Result += $"\nFound a match! {string.Join(",", byteArray)}\n";
                                Result += $"\nHash Type: {hashType}\n";

                                var fileInfo = new FileInfo(Environment.ProcessPath);
                                var directory = fileInfo.DirectoryName;
                                File.AppendAllText($"\nFound a match! {string.Join(",", byteArray)}\n", 
                                    $"${directory}/hashes.txt");
                                File.AppendAllText($"\nHash Type: {hashType}\n", 
                                    $"${directory}/hashes.txt");

                                _keepGoing = false;
                            }
                        }
                    });
                    
                    if (_keepGoing)
                    {
                        await context.Database.ExecuteSqlRawAsync(
                            $"DELETE FROM public.\"TB_PROCESS_QUEUE\" WHERE \"ID\" = '{item.Id}';");
                    }
                }
                else
                {
                    _keepGoing = false;
                }
            }
        }

        Result += "Done hashing byte arrays!" + Environment.NewLine;
        Processed = string.Empty;
    }

    [RelayCommand]
    private async void RegenerateDataset()
    {
        try
        {
            using (var context = new LiberContext())
            {
                await context.Database.EnsureCreatedAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        _ = Task.Run(() => GenerateAllByteArrays());
    }

    private async Task GenerateAllByteArrays()
    {
        using (var context = new LiberContext())
        {
            await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE public.\"TB_PROCESS_QUEUE\";");
        }

        _maxCombinations = BigInteger.Pow(256, int.Parse(MaxArrayLength));
        _currentCombinations = BigInteger.Zero;
        await GenerateByteArrays(int.Parse(MaxArrayLength));

        Result += "\nDone generating byte arrays!\n";
    }

    private async Task GenerateByteArrays(
        int maxArrayLength,
        int currentArrayLevel = 1,
        byte[]? passedArray = null)
    {
        if (!_keepGoing)
        {
            return;
        }
        
        if (currentArrayLevel == maxArrayLength)
        {
            await Parallel.ForAsync(0, 256, async (i, cancellationToken) =>
            {
                byte[] currentArray = new byte[currentArrayLevel];
                if (passedArray != null)
                {
                    Array.Copy(passedArray, currentArray, passedArray.Length);
                }

                currentArray[currentArrayLevel - 1] = (byte)i;

                var item = ProcessQueueItem.GenerateQueueItem(string.Join(",", currentArray));
                _tasks.Enqueue(item.GetHopperInsertString());
            });

            await ProcessTasks();
            
            _currentCombinations = BigInteger.Add(_currentCombinations, 256);
            _maxCombinations = BigInteger.Subtract(_maxCombinations, 256);
            
            if (BigInteger.Remainder(_currentCombinations, 256) == 0)
            {
                Result = $"Generating {MaxArrayLength} length byte arrays...\n{_currentCombinations} Computed\n{_maxCombinations} Remaining";
            }
        }
        else
        {
            byte[] currentArray = new byte[currentArrayLevel];
            if (passedArray != null)
            {
                Array.Copy(passedArray, currentArray, passedArray.Length);
            }

            for (int i = 0; i < 256; i++)
            {
                currentArray[currentArrayLevel - 1] = (byte)i;
                await GenerateByteArrays(maxArrayLength, currentArrayLevel + 1, currentArray);
            }
        }
        
        if (currentArrayLevel == 1)
        {
            await ProcessTasks();
        }
    }

    private async Task ProcessTasks()
    {
        if (_tasks.Count > 0)
        {
            using (var context = new LiberContext())
            {
                await context.Database.ExecuteSqlRawAsync(string.Join(";", _tasks));
                _tasks.Clear();
            }
        }
    }
}