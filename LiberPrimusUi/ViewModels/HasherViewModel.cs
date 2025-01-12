using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        if (RegenDataset)
        {
            using (var context = new LiberContext())
            {
                await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE public.\"TB_PROCESS_QUEUE\";");

                for (var i = 1; i <= int.Parse(MaxArrayLength); i++)
                {
                    Result += $"Generating {i}/{MaxArrayLength} length byte arrays..." + Environment.NewLine;
                    await GenerateByteArrays(context, i);
                }
            }

            Result += "Done generating byte arrays!" + Environment.NewLine;
            Result += "Starting to hash byte arrays..." + Environment.NewLine;
        }

        bool keepGoing = true;

        ulong counter = 0;

        while (keepGoing)
        {
            counter++;

            if (counter % 10000 == 0)
            {
                Processed = $"Processed {counter} items" + Environment.NewLine;
            }

            if (counter >= ulong.MaxValue - 100)
            {
                counter = 0;
            }

            using (var context = new LiberContext())
            {
                var item = context.ProcessQueueItems.FirstOrDefault();
                
                if (item != null)
                {
                    List<byte> byteArray = new List<byte>();
                    byteArray.AddRange(item.HopperString.Split(",").Select(
                        s =>
                        {
                            var byteInt = Convert.ToByte(s);
                            return byteInt;
                        }));

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
                            Result += $"Found a match! {item.HopperString}" + Environment.NewLine;
                            Result += $"Hash Type: {hashType}" + Environment.NewLine;
                            keepGoing = false;
                            break;
                        }
                    }

                    if (keepGoing)
                    {
                        await context.Database.ExecuteSqlAsync(
                            $"DELETE FROM public.\"TB_PROCESS_QUEUE\" WHERE \"ID\" = {item.Id};");
                    }
                }
                else
                {
                    keepGoing = false;
                }
            }
        }
        
        Result += "Done hashing byte arrays!" + Environment.NewLine;
    }

    private async Task GenerateByteArrays(LiberContext context, int maxArrayLength, int currentArrayLevel = 1,
        byte[]? passedArray = null)
    {
        byte[] currentArray = new byte[currentArrayLevel];
        if (passedArray != null)
        {
            for(int i = 0; i < passedArray.Length; i++)
            {
                currentArray[i] = passedArray[i];
            }
        }

        for (int i = byte.MinValue; i <= byte.MaxValue; i++)
        {
            currentArray[currentArrayLevel - 1] = (byte)i;

            if (currentArrayLevel == maxArrayLength)
            {
                var item = ProcessQueueItem.GenerateQueueItem(string.Join(",", currentArray));
                await context.Database.ExecuteSqlRawAsync(item.GetHopperInsertString());
            }
            else
            {
                await GenerateByteArrays(context, maxArrayLength, currentArrayLevel + 1, currentArray);
            }
        }
    }
}