.. -*- mode: rst -*-

==========================
 CLAutoThumbnailer README
==========================

**CLAutoThumbnailer** is a command-line program that makes it easy to
generate nicely sized thumbnails of a video file (or even entire
directories of video files). Instead of having to experiment with the
number of columns and rows until you get thumbnails that are not too
small or too large, **CLAutoThumbnailer** will do its best to figure
this out for you automatically.

You specify the number of rows (or columns) of thumbnails you want along
with the dimensions of the screen you'll be using to view the thumbnail
pages --- or just use the defaults --- and **CLAutoThumbnailer** will do
the rest. It automatically chooses the best number of columns (or rows)
that will fit.

It can create the typical *Overview Thumbnail Page* that has thumbnails
of an entire video on a single page. In addition, it can also create as
many *Detail Thumbnail Pages* as required to have thumbnails made every
set interval (for example, every 60 seconds).

Only in this way is it possible to process entire directories of video
files and still get nicely sized thumbs for all of them. This is
especially true if your collection contains videos of different aspect
ratios such as full-screen 1.33 (4:3), high-definition 1.77 (16:9),
wide-screen DVD 2.35, or something in-between. Without automatic
row/column determination, most of those types of files will come out
with badly sized thumbnail pages.

Features¶
=========

+ Allows fully “automatic” operation based on the video aspect ratio but
  is also customizable.

+ Can generate an Overview thumbnail page of an entire video.

+ Can generate Detail thumbnail pages where the thumbnails are created
  using a specified time interval.

+ Can process entire directories (and sub-directories).

+ Can process multi-part video files (like DVD .vob sets) as a single
  long video.

+ Allows setting starting and ending times for thumbnails.

+ Allows cropping to remove black bars and stretching to fix distortion.

+ Supports Command Files for batch processing of files requiring custom settings.

+ Allows creation of thumbnails at frequent intervals (sub-second) while
  still preserving thumbnail time accuracy (though some formats are better
  than others at this).

+ Is fully documented.

+ Has fully commented C# source code with documentation project files
  for Doxygen and Sandcastle Help File Builder. 


See the full documentation at: http://rm2.github.com/CLAutoThumbnailer/.

..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
