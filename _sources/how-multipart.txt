.. default-role:: filename

==============================
 Thumbnail a multi-part video
==============================

See :doc:`ref-multipart` for the details but basically some video files
are really meant to be watched as a single entity. That being the case,
you might also want to treat them as one group when generating
thumbnails.

Instead of using the :option:`-d, --directory <-d>` option as discussed
in :doc:`how-thumbnaildir` (which will treat each file separately),
specifying a ``videofilename`` with the wildcard character ``*`` in it
will treat all matching files as one "fileset".

Basic operation
===============

To demonstrate this I made a multi-part video by splitting up the 1.5GB
version of "McLintock! (1963)" into three pieces::

   C:\McLintock (1963)\multipart\
      McLintock_part01.mp4
      McLintock_part02.mp4
      McLintock_part03.mp4

You can find the |MI|_ info on these files :ref:`here
<sample-multi-part>`.

To generate a single Overview page and a single set of Detail pages for
these files do::

   cd "C:\McLintock (1963)\multipart"
   clatn "McLintock_part*.mp4"

which will tell you::

   Processing C:\McLintock (1963)\multipart\McLintock_part01.mp4 ...
   Processing C:\McLintock (1963)\multipart\McLintock_part02.mp4 ...
   Processing C:\McLintock (1963)\multipart\McLintock_part03.mp4 ...
   0:00:15 Total time to create AVFileSet.
   Thumbnails Range   0:00:05.000 -> 2:06:42.224
   Thumbnail Duration 2:06:37.224 (Total 2:06:47.224)
   Generating 84 181x78 thumbnails on a 7x12 Overview page.
   0:00:12 to create Overview thumbnails.
   84 thumbnails created. 0.14 seconds / thumbnail.
   Generating 762 452x195 thumbnails every 10 seconds on 51 3x5 Detail pages.
   0:01:34 to generate Detail page thumbnails.
   762 thumbnails created. 0.12 seconds / thumbnail.
   0:02:02 overall time to process McLintock_MULTI.mp4.

   0:02:02 Total time.

and create these thumbnail pages:

.. figure:: images/McLintock_MULTI_overview.jpg
   :scale: 50%
   :align: center
   :alt: McLintock_MULTI_.mpeg Thumbnail Page
   :target: _images/McLintock_MULTI_overview.jpg

   McLintock_MULTI_overview.jpg

.. figure:: images/McLintock_MULTI_page0001_00_00_05.jpg
   :scale: 50%
   :align: center
   :alt: McLintock_MULTI_.mpeg First Multi-Page Thumbnail Page
   :target: _images/McLintock_MULTI_page0001_00_00_05.jpg

   McLintock_MULTI_page0001_00_00_05.jpg

.. figure:: images/McLintock_MULTI_page0051_02_05_00.jpg
   :scale: 50%
   :align: center
   :alt: McLintock_MULTI_.mpeg Last Multi-Page Thumbnail Page
   :target: _images/McLintock_MULTI_page0051_02_05_00.jpg

   McLintock_MULTI_page0051_02_05_00.jpg

If you compare these pages with the ones generated in
:ref:`thumbnailing-widescreen`, you'll see that the actual thumbnail
images are identical.

There are some differences, however, that supply additional helpful
information when looking at thumbnail pages for multi-part videos.

The upper-left corner of the Overview page displays the number of files
in the video in addition to its total duration. The name of the video
has ``_MULTI`` added to it to further indicate that this page was made
from a composite of individual video files.

The first thumbnail from each part has the file number added to its
timestamp and is highlighted with a different colored border to make it
slightly easier to notice the location of the file splits. This makes it
possible to determine which file a thumbnail actually came from in case
you want to view the file in your media player.

The upper-left corner of the Detail pages also adds the display of the
number of files and _MULTI_ is added to the video name.

The thumbnail times indicate both the time from the actual start and the
time from the beginning of each individual file along with the file
number. The first thumbnail from each part is again be highlighted with
a different colored border.

Most (all?) media players aren't capable of jumping to the cumulative
time within a set of files (with the exception of the special case of
DVDs). Using the Detail page timestamps, however, you can open up the
correct file in a media player and jump to the exact time in the video
when the thumbnail was taken.


Skipping the credits of a multi-part video
==========================================

Of course, it's possible to use :doc:`Command-Line Options
<ref-options>` to customize the thumbnail generation of multi-part
videos. For example, here's how you would skip the credits on the
Overview thumbnail page::

   clatn -i 0 -s 0:1:59 -e 2:06:39 "McLintock_part*.mp4"

with this result:

.. figure:: images/McLintock_MULTI_nocredits_overview.jpg
   :scale: 50%
   :align: center
   :alt: "No Credits" Overview Thumbnail Page for Multi-Part "McLintock!"
   :target: _images/McLintock_MULTI_nocredits_overview.jpg

   "No Credits" Overview Thumbnail Page for Multi-Part "McLintock!"

If you compare this with the page generated in :doc:`how-skipcredits`,
you'll again see that the actual thumbnails are identical.


..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
