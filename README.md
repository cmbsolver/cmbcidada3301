# Liber Primus Analysis Tool
This is my Liber Primus Analysis Tool.  I have been using it for investigating the Liber Primus.  It is early in its development and I am working on it as I have the time to do it.  I have also used it on some other puzzles so its application is not limited to the Liber Primus.

# Running the tool from source
You will need to download the .Net SDK in order to get started (https://dotnet.microsoft.com/).

Once you have the SDK installed, go into the LiberPrimusUi folder and execute the following commands.

1. `dotnet restore`
2. `dotnet run`

This will bring up the user interface. The left hand side has the menu options of what can be run.

# Releases
I do release binaries every so often for the main 3 OSes.  You are more than welcome to use them, but I do not have hardware to test it out.  If you encounter and issue, please feel free to open an issue.  I will get to it as time permits.

# Libs and Whatnot
This was written using C# using .Net 8 with Mediatr, Avalonia, Six Labors, Image Magick, and other libraries.  It is meant to be runnable on Windows, Mac, and Linux.  I do not have the other stuff added to compile to mobile or web, but you may be able to add that if you want to.
