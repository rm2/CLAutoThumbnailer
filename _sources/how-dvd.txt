.. default-role:: filename

==============================
 Thumbnail an unencrypted DVD
==============================

Thumbnailing an unencrypted :doc:`DVD <ref-mpeg2>` is conceptually no
different than :doc:`how-multipart` but since it might be a popular
thing to do this section will discuss some additional details.

Basic operation
===============

To demonstrate thumbnailing a DVD I used `DVDStyler
<http://www.dvdstyler.de/en/>`_ on `his_girl_friday.mpeg` to create a
single title DVD. It created the following files in a `VIDEO_TS`
directory::


   C:\His Girl Friday (1940)\dvd\VIDEO_TS\
      VIDEO_TS.BUP
      VIDEO_TS.IFO
      VIDEO_TS.VOB
      VTS_01_0.BUP
      VTS_01_0.IFO
      VTS_01_0.VOB
      VTS_01_1.VOB
      VTS_01_2.VOB
      VTS_01_3.VOB
      VTS_01_4.VOB

You can find the |MI|_ info on the `VTS_01_*.VOB` files :ref:`here
<sample-dvd>`.

By default, thumbnails for a video are written to the original directory of
that video. If you are generating thumbnails directly from `VOB` files
that reside on a DVDROM drive, you have to use the :option:`-o, --outdir
<-o>` option to tell |CLATN| where to write the thumbnail pages.

In addition, you have to figure out which "title set" you want to
thumbnail. Title sets look like ``VTS_TT_NN.VOB`` where ``TT`` is the
title number and ``NN`` is the part within that title. Normally you'll
want to do the largest title set and it is also normally the first one,
so you'll specify ``VTS_01_*,VOB``. If you are unsure, with an
unencrypted DVD, you can just watch the first few seconds of each title
set (skip the ``00`` parts since those are for the DVD menu), to figure
out what title you want.

Finally, in the special case of VOB files only, since the actual file
names aren't very informative, |CLATN| will add the output directory name
to the filenames.

If the above ``VIDEO_TS`` had been written to a DVD+R/DVD-R disc, you
would therefore have to do something like::

   cd "C:\His Girl Friday (1940)"
   clatn -o . e:\VIDEO_TS\VTS_01_*.VOB"

Even when the `VOB`\ s reside on a hard drive (a much faster operation),
you'll still probably want to put the thumbs in a different directory,
so do::

   cd "C:\His Girl Friday (1940)"
   clatn -o . "C:\His Girl Friday (1940)\dvd\VIDEO_TS\VTS_01_*.VOB"

which will tell you::

   Processing C:\His Girl Friday (1940)\dvd\VIDEO_TS\VTS_01_1.VOB ...
   Processing C:\His Girl Friday (1940)\dvd\VIDEO_TS\VTS_01_2.VOB ...
   Processing C:\His Girl Friday (1940)\dvd\VIDEO_TS\VTS_01_3.VOB ...
   Processing C:\His Girl Friday (1940)\dvd\VIDEO_TS\VTS_01_4.VOB ...
   0:03:59 Total time to create AVFileSet.
   Thumbnails Range   0:00:05.000 -> 1:31:39.298
   Thumbnail Duration 1:31:34.298 (Total 1:31:44.298)
   OutputDirectory is "."
   Auto adjusting aspect ratio from 1.500 to 1.333
   Generating 144 104x78 thumbnails on a 12x12 Overview page.
   0:01:18 to create Overview thumbnails.
   144 thumbnails created. 0.55 seconds / thumbnail.
   Generating 552 325x244 thumbnails every 10 seconds on 35 4x4 Detail pages.
   0:04:04 to generate Detail page thumbnails.
   551 thumbnails created. 0.44 seconds / thumbnail.
   0:09:23 overall time to process thumbs - vts_01.dvd.

   0:09:26 Total time.

Notice that |CLATN| automatically skips `VTS_01_00.VOB` which is just
the menu page for the title and not part of the movie. Here's a sample
of the generated thumbnail pages:

.. figure:: images/His_Girl_Friday_(1940)_-_vts_01_overview.jpg
   :scale: 50%
   :align: center
   :alt: his_girl_friday dvd Overview Thumbnail Page
   :target: _images/His_Girl_Friday_(1940)_-_vts_01_overview.jpg

   "His Girl Friday" DVD Overview Thumbnail Page

.. figure:: images/His_Girl_Friday_(1940)_-_vts_01_page0001_00_00_05.jpg
   :scale: 50%
   :align: center
   :alt: his_girl_friday dvd First Detail Thumbnail Page
   :target: _images/His_Girl_Friday_(1940)_-_vts_01_page0001_00_00_05.jpg

   "His Girl Friday" DVD First Detail Thumbnail Page

.. figure:: images/His_Girl_Friday_(1940)_-_vts_01_page0035_01_30_40.jpg
   :scale: 50%
   :align: center
   :alt: his_girl_friday dvd Last Detail Thumbnail Page
   :target: _images/His_Girl_Friday_(1940)_-_vts_01_page0035_01_30_40.jpg

   "His Girl Friday" DVD Last Detail Thumbnail Page

The information added to the thumbnail pages of multi-part files is
discussed in :doc:`how-multipart`. However, since media players are
either not able to jump to a specific time, or are able to jump to the
time within a DVD title (rather than the time from the start of a `VOB`
file within a DVD title) this additional information isn't particularly
useful for DVDs.

Skipping the credits of a DVD title
===================================

Of course, it's possible to use :doc:`Command-Line Options
<ref-options>` to customize the thumbnail generation of DVDs. For
example, here's how you would skip the credits on the Overview thumbnail
page::

   clatn -o . -i 0 -s 0:0:48 -e 1:31:07.184 "C:\His Girl Friday (1940)\dvd\VIDEO_TS\VTS_01_*.VOB"

with this result:

.. figure:: images/His_Girl_Friday_(1940)_-_nocredits_vts_01_overview.jpg
   :scale: 50%
   :align: center
   :alt: "No Credits" Overview Thumbnail Page for "His Girl Friday" DVD
   :target: _images/His_Girl_Friday_(1940)_-_nocredits_vts_01_overview.jpg

   "No Credits" Overview Thumbnail Page for "His Girl Friday" DVD

..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
