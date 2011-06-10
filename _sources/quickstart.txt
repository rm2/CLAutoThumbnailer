.. default-role:: filename

=============
 Quick Start
=============

The following assumes you've downloaded the public domain
`his_girl_friday_512kb.mp4` and `McLintock_512kb.mp4` videos mentioned
in the :doc:`samplefile` section but you can substitute any other video
files.

Basic Operation
===============

Open up a Command Prompt window and type the following::

   cd "C:\His Girl Friday (1940)\mp4"
   clatn "his_girl_friday_512kb.mp4"

you'll see::

   Processing C:\His Girl Friday (1940)\mp4\his_girl_friday_512kb.mp4 ...

and after a some seconds (depending on how long it takes your computer
to open up a 382.6MB file) you'll see::

   0:00:11 Total time to create AVFileSet.
   Thumbnails Range   0:00:05.000 -> 1:31:39.521
   Thumbnail Duration 1:31:34.521 (Total 1:31:44.521)
   Generating 144 104x78 thumbnails on a 12x12 Overview page.

and then an "animation" that changes every time a new overview thumbnail
is created followed by a percent complete indicator. When this is done
you'll see::

   0:00:13 to create Overview thumbnails.
   144 thumbnails created. 0.09 seconds / thumbnail.

and then Detail page thumbnail generation starts::

   Generating 552 325x244 thumbnails every 10 seconds on 35 4x4 Detail pages.

and again an "animation" is displayed that changes every time a new
thumbnail is created followed by a percent complete indicator. When it's
done, you'll see::

   0:00:48 to generate Detail page thumbnails.
   551 thumbnails created. 0.09 seconds / thumbnail.
   0:01:13 overall time to process his_girl_friday_512kb.mp4.
   0:01:13 Total time.

At this point you'll have the following new files in the
`C:\\His Girl Friday (1940)\\mp4` folder::

   CLAutoThumbnailer.log
   his_girl_friday_512kb_overview.jpg
   his_girl_friday_512kb_page0001_00_00_05,jpg
   his_girl_friday_512kb_page0002_00_02_40.jpg
    ...
   his_girl_friday_512kb_page0035_01_30_40.jpg

The three sets of numbers at the end of the detail page filenames are
the time of the first thumbnail on that page in
``hours_minutes_seconds`` format. This makes it easy to find thumbnails
at a particular time in a video.

Here's a sample of what the generated thumbnail pages look like (click
on the images to see them at full scale):

.. figure:: images/his_girl_friday_512kb_overview.jpg
   :scale: 50%
   :align: center
   :alt: his_girl_friday_512kb_.mpeg Thumbnail Page
   :target: _images/his_girl_friday_512kb_overview.jpg

   his_girl_friday_512kb_overview.jpg

.. figure:: images/his_girl_friday_512kb_page0001_00_00_05.jpg
   :scale: 50%
   :align: center
   :alt: his_girl_friday_512kb_.mpeg First Detail Thumbnail Page
   :target: _images/his_girl_friday_512kb_page0001_00_00_05.jpg

   his_girl_friday_512kb_page0001.jpg

.. figure:: images/his_girl_friday_512kb_page0035_01_30_40.jpg
   :scale: 50%
   :align: center
   :alt: his_girl_friday_512kb_.mpeg Last Detail Thumbnail Page
   :target: _images/his_girl_friday_512kb_page0035_01_30_40.jpg

   his_girl_friday_512kb_page0035.jpg

We can see that the thumbnail timestamp is very accurate by comparing
the thumbnail generated at :mono:`01:30:40` to the frame displayed at
that time by |EE|_ and |BSP|_:

.. figure:: images/ee4-hgf-mp4-ff-01m30s40.png
   :scale: 50%
   :align: center
   :alt: Frame at 1:30:40 using Microsoft Expression Encoder 4
   :target: _images/ee4-hgf-mp4-ff-01m30s40.png

   Frame at 1:30:40 using Microsoft Expression Encoder 4

.. figure:: images/bsp-hgf-mp4-ff-01m30s40.png
   :scale: 50%
   :align: center
   :alt: Frame at 1:30:40 using BS.Player
   :target: _images/bsp-hgf-mp4-ff-01m30s40.png

   Frame at 1:30:40 using BS.Player

`CLAutoThumbnailer.log` is just a text file that stores details on the
thumbnailing operation. It contains::

   Command Line: his_girl_friday_512kb.mp4
   Processing C:\His Girl Friday (1940)\mp4\his_girl_friday_512kb.mp4 ...
   0:00:10.14 to create Microsoft.Encoder.AudioVideoFile.
   0:00:00.67 to create Microsoft.Encoder.MediaItem.
   0:00:01.10 to run CalculateDuration.
   AudioVideoFile.Duration 1:31:44.5211 C:\His Girl Friday (1940)\mp4\his_girl_friday_512kb.mp4.
   VideoStream.Duration    1:31:44.5211 C:\His Girl Friday (1940)\mp4\his_girl_friday_512kb.mp4.
   AudioStream.Duration    1:31:44.5211 C:\His Girl Friday (1940)\mp4\his_girl_friday_512kb.mp4.
   AudioStream channels =  6 C:\His Girl Friday (1940)\mp4\his_girl_friday_512kb.mp4.
   0:00:11 Total time to create AVFileSet.
   Using Auto Layout Mode.
   Thumbnails Range   0:00:05.000 -> 1:31:39.521
   Thumbnail Duration 1:31:34.521 (Total 1:31:44.521)
   Auto Row Priority Layout (aspect ratio 1.33 >= 1.275), using 12 rows
     1.33 aspect ratio
     Wasted width 0.733 > 0.600, increased columns:  12x12 0.000x0.326 
   Generating 144 104x78 thumbnails on a 12x12 Overview page.
   0:00:13 to create Overview thumbnails.
   144 thumbnails created. 0.09 seconds / thumbnail.
   Auto Row Priority Layout (aspect ratio 1.33 >= 1.275), using 4 rows
     1.33 aspect ratio
     Wasted width 0.884 > 0.600, increased columns:  4x4 0.000x0.137 
   Generating 552 325x244 thumbnails every 10 seconds on 35 4x4 Detail pages.
   0:00:48 to generate Detail page thumbnails.
   551 thumbnails created. 0.09 seconds / thumbnail.
   0:01:13 overall time to process his_girl_friday_512kb.mp4.

   0:01:13 Total time.


but for the most part you can just ignore it.

.. _thumbnailing-widescreen:

Thumbnailing a Widescreen Video
===============================

`his_girl_friday_512kb.mp4` is a "Standard" (aka "Fullscreen")
video. Let's see how |CLATN| handles a "Widescreen" video by generating
thumbnails for `McLintock_512kb.mp4`. Do::

   cd "C:\McLintock (1963)\mp4 512kb"
   clatn "McLintock_512kb.mp4"

and you'll see::

   Processing C:\McLintock (1963)\mp4 512kb\McLintock_512kb.mp4 ...
   0:00:11 Total time to create AVFileSet.
   Thumbnails Range   0:00:05.000 -> 2:06:42.214
   Thumbnail Duration 2:06:37.214 (Total 2:06:47.214)
   Generating 84 181x78 thumbnails on a 7x12 Overview page.
   0:00:09 to create Overview thumbnails.
   84 thumbnails created. 0.12 seconds / thumbnail.
   Generating 762 452x195 thumbnails every 10 seconds on 51 3x5 Detail pages.
   0:01:27 to generate Detail page thumbnails.
   762 thumbnails created. 0.12 seconds / thumbnail.
   0:01:49 overall time to process McLintock_512kb.mp4.
   0:01:49 Total time.

And here's some samples of what the generated thumbnail pages look like
(click on the images to see them at full scale):

.. figure:: images/McLintock_512kb_overview.jpg
   :scale: 50%
   :align: center
   :alt: McLintock_512kb_.mpeg Thumbnail Page
   :target: _images/McLintock_512kb_overview.jpg

   McLintock_512kb_overview.jpg

.. figure:: images/McLintock_512kb_page0001_00_00_05.jpg
   :scale: 50%
   :align: center
   :alt: McLintock_512kb_.mpeg First Detail Thumbnail Page
   :target: _images/McLintock_512kb_page0001_00_00_05.jpg

   McLintock_512kb_page0001_00_00_05.jpg

.. figure:: images/McLintock_512kb_page0051_02_05_00.jpg
   :scale: 50%
   :align: center
   :alt: McLintock_512kb_.mpeg Last Detail Thumbnail Page
   :target: _images/McLintock_512kb_page0051_02_05_00.jpg

   McLintock_512kb_page0051_02_05_00.jpg

The `CLAutoThumbnailer.log` for this operation contains::

   Command Line: McLintock_512kb.mp4
   Processing C:\McLintock (1963)\mp4 512kb\McLintock_512kb.mp4 ...
   0:00:10.01 to create Microsoft.Encoder.AudioVideoFile.
   0:00:00.64 to create Microsoft.Encoder.MediaItem.
   0:00:01.20 to run CalculateDuration.
   AudioVideoFile.Duration 2:06:47.2143 C:\McLintock (1963)\mp4 512kb\McLintock_512kb.mp4.
   VideoStream.Duration    2:06:47.2143 C:\McLintock (1963)\mp4 512kb\McLintock_512kb.mp4.
   AudioStream.Duration    2:06:47.2143 C:\McLintock (1963)\mp4 512kb\McLintock_512kb.mp4.
   AudioStream channels =  6 C:\McLintock (1963)\mp4 512kb\McLintock_512kb.mp4.
   0:00:11 Total time to create AVFileSet.
   Using Auto Layout Mode.
   Thumbnails Range   0:00:05.000 -> 2:06:42.214
   Thumbnail Duration 2:06:37.214 (Total 2:06:47.214)
   Auto Row Priority Layout (aspect ratio 2.32 >= 1.275), using 12 rows
     2.32 aspect ratio
     Wasted width 0.852 > 0.600, increased columns:  7x12 0.000x0.315 
   Generating 84 181x78 thumbnails on a 7x12 Overview page.
   0:00:09 to create Overview thumbnails.
   84 thumbnails created. 0.12 seconds / thumbnail.
   Auto Row Priority Layout (aspect ratio 2.32 >= 1.275), using 4 rows
     2 columns < minimum, setting to 3
     2.32 aspect ratio
     3 columns is good enough, wasted width 0.000 <= 0.600
     3x4 0.000x1.419
     Now wasted height 1.419 > 0.600, increased rows:  3x5 0.000x0.369 
   Generating 762 452x195 thumbnails every 10 seconds on 51 3x5 Detail pages.
   0:01:27 to generate Detail page thumbnails.
   762 thumbnails created. 0.12 seconds / thumbnail.
   0:01:49 overall time to process McLintock_512kb.mp4.
   0:01:49 Total time.


Some Details
============

The first thing you should notice is that while |CLATN| creates the
default 12 Overview rows for both videos, it decides to use 12 columns
for "His Girl Friday" and only 7 for "McLintock!". It does this to keep
the thumbnails the same height. Since "McLintock!"s widescreen
thumbnails are so much wider than "His Girl Friday"s fullscreen ones, it
has to reduce the number of columns to make them fit.
       
On the Detail pages, notice that the default 4 rows are used for "His
Girl Friday" but 5 are used for "McLintock!". When 4 rows are used only
2 columns will fit which is below the default minimum of 3 columns. So
|CLATN| bumps up the number of columns to 3, but then sees that this
wastes too much vertical space and also increases the number of rows to
5.

See :doc:`ref-layout` for more information.

What's Next?
============

|CLATN| has many options you can use to control its operation. See
:ref:`command-line-options` for the entire list or see the :doc:`howto`
section for instructions on generating various results.



..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
