.. default-role:: filename

==============================
 Auto Aspect Ratio Adjustment
==============================

The :option:`--aar` Auto Aspect Ratio adjustment option can
automatically remove the blacks bars from video thumbnails. Some video
files (usually :doc:`ref-mpeg2`) have their “Display aspect ratio”
property set to something different than the aspect ratio of their video
frames. The :option:`--aar` option works by automatically calling the
:option:`-p, --crop <-p>` option for you with the video's display aspect
ratio.


You can see this, for example, in the |MI|_ report for the :mono:`MPEG2`
version of :ref:`His Girl Friday <mediainfo-his_girl_friday-mpeg>`::

   Width                            : 720 pixels
   Height                           : 480 pixels
   Display aspect ratio             : 4:3

where the video frame aspect ratio is 1.5 (720 / 480) and the Display
aspect ratio is 1.33 (4 / 3). Compare this to the report for the
:ref:`MPEG4 version <mediainfo-his_girl_friday_512kb-mp4>`::

   Width                            : 320 pixels
   Height                           : 240 pixels
   Display aspect ratio             : 4:3

where the video frame aspect ratio is 1.33 (320 / 240) is the same as
the Display aspect ratio.

To see the :option:`--aar` option in action look at the following
thumbnails that were generated with ``aar`` Off and On:

.. table::
   :class: centered, centercells, noborders

   +--------------+------------+
   |   |noaar|    |   |aar|    |
   +--------------+------------+
   | **aar Off**  | **aar On** |
   +--------------+------------+

.. |noaar| image:: images/his_girl_friday_page0001_00_00_05_noaar.jpg
           :scale: 25%
           :alt: ``--aar-`` Off
           :target: _images/his_girl_friday_page0001_00_00_05_noaar.jpg

.. |aar| image:: images/his_girl_friday_page0001_00_00_05_aar.jpg
         :scale: 25%
         :alt:  ``--aar`` On
         :target: _images/his_girl_friday_page0001_00_00_05_aar.jpg

With ``aar`` Off you can see that the :mono:`MPEG2` version of "His Girl
Friday" is indeed pillarboxed, as we expect from its MediaInfo
report. You don't normally notice this since most media players honor
the display aspect ratio property and will automatically crop the video
during playback. |CLATN| emulates this same behavior by normally running
with ``aar`` enabled.

If you look at the full-size images by clicking on the above thumbnails,
you'll see that the "aar Off" version says in the upper-right corner
``720x480 (1.50:1)``, which is the video frame size and its aspect
ratio.

The "aar On" version has ``640x480 (1.33:1) [720x480 (1.50:1)]``. When
you have two values for the frame size and aspect ratio, one inside
square brackets, it means the thumbnail frames are taken from something
other than the full original video frames. The dimensions inside the
square brackets are the original dimensions, while the other numbers are
the actual frame and aspect ratio used to create the thumbnails. Notice
that the black bars have been removed.

Letterboxing or pillarboxing usually occurs when the display aspect
ratio isn't set or is set incorrectly. Luckily, as :doc:`how-blackbars`
shows, it's a (mostly) simple matter to fix this problem by using the
:option:`-p, --crop <-p>` option explicitly.


..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
