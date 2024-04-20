# Question
Is there something hidden within the bytes or pixels of the unsolved images?

# Description
I wrote the code to do this in C#.  I can use languages like Python, C/C++, etc.  However, it has some of the better tooling that makes me productive.  I have a day job, so my time is limited.  And I thought this was fun since I don’t do steganographic or cryptographic analysis for my day job.

# Tooling
.Net 8 / C# with the Mediatr, ImageMagick, and Spectre Console libs.  I wanted to keep this cross platform, so I didn’t use any tools limited to Windows or Mac.  And I didn’t want to do yet another web application.  Though, since it is using Mediatr, if someone wanted to build a web app or other application around it, it is certainly possible.

I called Liber Primus Analysis Tool.  I should have named it Cryptographic Research Analysis Program.

Either way, I started working on this in March 2024, so it is an early work in progress.  There are many more nooks, crannies, and cul-de-sacs to go down.

# My Thoughts?
I know 3301 has used stuff like outguess in the past.  However, I want to hope they tried some other mechanics for hiding stuff in images since outguessing hasn’t really turned up anything.  The first thing I did was create a color report to see how many colors we are talking about here.
There are quite a few colors on each image.

# Investigations
1. Did a color count as I have used this before to create images.  Basically, I would get the array of pixels and get the count each time the color switched up.  Then I would convert that number into a byte if it was under 255.  Then I would dump that byte array to a file.  I did this forward and backwards.  I ran them through a lib to identify binaries from magic numbers.
Result: Bupkis

2. Variance 2 of the color count was to do the counts by color.
Result: Also, bupkis

3. Byte Winnowing – This is a technique where we still go byte by byte.  Winnowing means that only certain bytes have significance.  I winnowed with Natural, Totient members, Cake, Prime, Fibonacci, and Binomial numeric sequences.  I also tried to shift the sequence down by 1 to start counting at one.  I used 1, 2, 3, and 8 (whole byte) least significant bits.
Result: I like this word “bupkis” for this effort.

4. Pixel Winnowing – Similar to bytes, but we do this with pixels.
Result: Nothing of value.  Which is the definition of bupkis.
