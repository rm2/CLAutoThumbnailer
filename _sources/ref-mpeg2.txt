.. default-role:: filename

======================
 MPEG2 Encoded Videos
======================

The :mono:`MPEG2` format, which is used in DVD :mono:`.VOB` files, has a
number of peculiarities worth mentioning.

The |EESDK| --- which |CLATN| uses to actually generate thumbnails ---
needs to do something it calls "Indexing" before it can fully open
:mono:`MPEG2` encoded video files. This adds an extra step that can be
pretty long. This is reported in `CLAutoThumbnailer.log` (or
:option:`--debug` option output) as the time ``to run
CalculateDuration`` line.

:mono:`MPEG2` doesn't seem to be as good at exact time positioning as
other video formats. In particular you will see small errors in the
timestamps of multi-part :mono:`MPEG2` encoded video files (like DVD
title sets, `VTS_NN_*.VOB`).

:mono:`MPEG2` videos can have a "Display aspect ratio" specified for
them that is different than their typical 720x480 (1.50:1) video frame
size. This is how DVDs support the display of widescreen movies. The
:option:`--aar` option controls whether the "Display aspect ratio" is
used to automatically crop thumbnails.

Thumbnailing directly from an unencypted DVDROM is :bi:`much` slower
than thumbnailing from a copy on your hard drive (because of the vastly
slower speed of DVDROM drives). If you plan on processing files that are
on a DVDROM more than once or twice, it's probably worthwhile to make a
copy if you have the room. Also some DVDROM drives will automatically
run slower to make them quieter while watching movies. You might want to
search the Internet for free utility programs that can force your DVDROM
drive to run at its maximum speed.

..
   local Variables:
   coding: utf-8
   mode: rst
   End:
