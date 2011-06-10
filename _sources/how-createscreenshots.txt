.. default-role:: filename

====================
 Create screenshots
====================

|CLATN| is designed to make thumbnail :bi:`pages` not screenshots (frame
grabs) of individual video frames. If you want to create screenshots,
you're best bet is to use `ffmpeg <http://www.ffmpeg.org/>`_. For
example, the following command::

   ffmpeg -i his_girl_friday_512kb.mp4 -r 1 -s 320x240 -ss 00:02:00 -t 60 -f image2 thumbs%08d.jpg

will generate 60 seconds of screenshots at 1 screenshot per second,
starting from 2 minutes into `his_girl_friday_512kb.mp4`.

The analogous operation with |CLATN| would be::

   clatn -i 1 -s 0:2:00 -e 0:2:59 -v- -R 1 -C 1 --layout 1 -w 326 -h 281 -l 0 "his_girl_friday_512kb.mp4"

where you have to explicitly say you want only one column and one row by
specifying ``-R 1 -C 1 --layout 1``, ``-w 326 -h 281`` gives a thumbnail
width and height of 320x240 after taking into account the margins,
border, and header line, and ``-l 0`` turns off the timestamps. This
results in the following messages::

   Processing C:\His Girl Friday (1940)\mp4\his_girl_friday_512kb.mp4 ...
   0:00:11 Total time to create AVFileSet.
   Thumbnails Range   0:02:00.000 -> 0:02:59.000
   Thumbnail Duration 0:00:59.000 (Total 1:31:44.521)
   Overview page skipped.
   Generating 60 320x240 thumbnails every 1 seconds on 60 1x1 Detail pages.
   0:00:11 to generate Detail page thumbnails.
   60 thumbnails created. 0.20 seconds / thumbnail.
   0:00:23 overall time to process his_girl_friday_512kb.mp4.

   0:00:23 Total time.

with sample thumbs that look like:

.. figure:: images/his_girl_friday_512kb_page0022_00_02_21.jpg
   :align: center
   :alt: Sample screenshot 1

   Sample screenshot 1

.. figure:: images/his_girl_friday_512kb_page0025_00_02_24.jpg
   :align: center
   :alt: Sample screenshot 2

   Sample screenshot 2

where |CLATN| has a problem since the thumbnail page isn't wide enough
to contain the information it displays on its header line.

If you don't mind doubling the dimension of your screenshots, you'll get
somewhat better results for tiny videos by doing::

   clatn -i 1 -s 0:2:00 -e 0:2:59 -v- -R 1 -C 1 --layout 1 -w 646 -h 521 -l 0 "his_girl_friday_512kb.mp4"

.. figure:: images/his_girl_friday_512kb_2x_page0022_00_02_21.jpg
   :align: center
   :alt: 2x Sample screenshot 1

   2x Sample screenshot 1

.. figure:: images/his_girl_friday_512kb_2x_page0025_00_02_24.jpg
   :align: center
   :alt: 2x Sample screenshot 2

   2x Sample screenshot 2


..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
