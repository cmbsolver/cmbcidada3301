# CMBsolver's Liber Primus Analysis Tool
This is my Liber Primus Analysis Tool.  I have been using it for investigating the Liber Primus.  It is **very early** in its development and I am working on it as I have the time to do it.  I have also used it on some other puzzles so its application is not limited to the Liber Primus.

## Running the tool from source
You will need to download the .Net SDK in order to get started (https://dotnet.microsoft.com/).

Once you have the SDK installed, go into the LiberPrimusUi folder and execute the following commands.

1. `dotnet restore`
2. `dotnet run`

This will bring up the user interface. The left hand side has the menu options of what can be run.

## Building release binaries
You can build the release binaries by running the following command in the LiberPrimusUi folder.

This is for Linux and not Windows or Mac.

```
chmod 777 build-output.sh
./build-output.sh
```

- Option 1: This will build the binaries for Windows, Mac, and Linux.  The binaries will be in the output folder.

- Option 2: This will clean up the output folder and perform a dotnet clean.

**This will not destroy the database!**

## Releases
I do release binaries every so often for the main 3 OSes.  You are more than welcome to use them, but I do not have hardware to test it out.  If you encounter an issue, please feel free to open an issue.  I will get to it as time permits.

If you do want to use the tool, you can download the latest release from the releases page.  You will need to download the zip file and extract it.  Once extracted, you can run the executable.  The OS is labeled in the zip file name.

The executable is LiberPrimusUi.exe.  You can run it from the command line or by double-clicking on it.

## Features
- Prime Checker
- Numeric Sequence Generator
- Latin and Rune Transposition
- Rune sentence sum utility.
- Getting words for decimal values (comma separated supported)
- Base64 decoder (file, text, and binary)
- Text to Binary
- Binary Identifier
- Binary data inversion
- Skip and take
- Word length dictionary checker
- Clock Angle Calculator
- Some letter frequency analysis tools
- Tools to Scytale text
- Tools to do a Caesar Cipher
- Tool to square and spiral text
- Color Report
- Color Inversion
- Byte and Pixel winnowing and LSB calculation

## Frequency Analysis - Database Required!!!
The letter frequency analysis tool requires a Postgres database to be present.

I prefer to use podman to host the database.  You can use docker if you want.  You can use the following command to start the database for podman.

### Powershell
You will need to install Python 3.9+ and Podman.  You can get Podman from https://podman.io/docs/installation.  You can get Python from https://www.python.org/downloads/.
```
pip3 install podman-compose
.\create_podman.ps1
```

### Bash
```
chmod 777 create_podman_db.sh
./create_podman_db.sh
```

If you use alternate credentials, you will need to modify the connection string in the connstring.txt file.  Otherwise, the script specifies the DB credentials.  The application will create the database upon first use of the analysis tools.

**The database is not required to run the application.  It is only required for the frequency analysis tool.**
**Reindexing the document will take a long time.**
**Reindexing will cause the database to be destroyed and recreated.  Anything you have created in the liberdb will be lost.**

## Libraries
This was written using C# using .Net 9 with Mediatr, Avalonia, Six Labors, Image Magick, and other libraries.  It is meant to be runnable on Windows, Mac, and Linux.  I do not have the other stuff added to compile to mobile or web, but you may be able to add that if you want to.

## Documents
There are the LP documents in the input folder.
- Images/LP: This is the Liber Primus images for the unsolved pages.
- Text/LP-Combined-Sections: This is the Liber Primus text for the unsolved pages in runes in combined sections.
- Text/LP-Combined-Sections-IRL: This is the Liber Primus text for the unsolved pages in Latin characters.
- Text/LP-Full: This is the Liber Primus rune text for the unsolved pages.
- Text/LP-Full-NLB: This is the Liber Primus rune text for the unsolved pages with no line breaks.