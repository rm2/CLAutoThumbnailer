.. default-role:: filename

===================================
 FAQ -- Frequently Asked Questions
===================================

.. contents::
   :local:

General
=======

1. Do I need to buy the Pro_ version of Microsoft Expression Encoder to
   run |CLATN|?

   If you're just going to be generating thumbnails (and not encoding
   videos), then you only need to install the `free version
   <http://www.microsoft.com/Expression/try-it/>`_ of Expression
   Encoder. You will also probably need some sort of supplemental codec
   package (like the |KLITE|_) to thumbnail more video file types.

#. Why doesn't |CLATN| display the video codec name in the thumbnail
   page header?

   I haven't found any way yet to get that information using the
   |EESDK|_. I may at some future time use some other technique to get
   this information.

#. Why doesn't |CLATN| display the number of audio channels in the
   thumbnail page header?

   The information returned by the |EESDK|_ seems to be usually wrong so
   I just skip it. It usually reports ``6`` channels even when there are
   only ``2``. I may at some future time use some other technique to get
   this information.

#. How I determine which codecs/filters |CLATN| is using to generate
   thumbnails for a particular video file?

   Unfortunately, the |EESDK|_ gives no way of determining this
   information --- since |CLATN| intimately depends on the SDK there is
   no other way to figure this out.

   The only possible solution is to use the Microsoft Expression Encoder
   user interface to reduce the number of enabled filters until there is
   only a single filter it :bi:`can` be using. See
   :ref:`ee-video-filters` for more information.

#. Where's the source code for |CLATN|?

   It can be found on github at: https://github.com/rm2/CLAutoThumbnailer

#. Where do I report bugs?

   Report bugs or suggestions for improvements at
   https://github.com/rm2/CLAutoThumbnailer/issues\ .


Downloading & Installation
==========================

1. Where can I download |CLATN|?

   The installer is at:
   https://github.com/downloads/rm2/CLAutoThumbnailer/CLAutoThumbnailerSetup-v1.0-20110610.msi

   A zip file archive is at:
   https://github.com/downloads/rm2/CLAutoThumbnailer/CLAutoThumbnailerSetup-v1.0-20110610.zip


#. How do I install |CLATN|?

   See :ref:`installation-instructions`.

   If you've already installed |CLATN| by executing
   `CLAutoThumbnailerSetup.msi`, you might see a *"Another version of
   this product is already installed. Installation of this version
   cannot continue. To configure or remove the existing version of this
   product, use Add/Remove Programs on the Control Panel."* message. You
   can either do what it says or run the following from a Command Prompt
   window::

      rename CLAutoThumbnailerSetup-v1.0-20110611.msi CLAutoThumbnailerSetup.msi
      msiexec /fav CLAutoThumbnailerSetup.msi

#. How do I tell what version of |CLATN| I'm running?

   Run the following command::

      clatn --version

#. How do I uninstall |CLATN|?

   Just use the Control Panel and uninstall like any other program.



Operation
=========

#. How do I see what |CLATN|’s default settings are?

   Just enter :command:`clatn` without any arguments or use the the
   :option:`-?, --help <--help>` option. The default settings are shown
   within square brackets (``[]``).

#. How do I generate just a standard Overview thumbnail page?

   Use the :option:`-i, --interval <-i>` option with an
   :optionarg:`interval` of ``0`` to avoid generating the Detail
   thumbnails.

#. How do I :bi:`not` generate an Overview thumbnail page?

   Use the :option:`-v, --overview <-v>` option by specifying ``-v-`` or
   ``--overview-``.

#. Why don't any of my video files get processed when using the
   :option:`-d, --directory <-d>` option?

   Make sure that your video file extensions are in the list of valid
   extensions that follow the :option:`-d, --directory <-d>` option
   shown using :option:`-?, --help <--help>`. Add any missing extensions
   using the :option:`--exts <--exts>` and :option:`--save <--save>`
   options.

   Make sure the :option:`--minsize` option is set correctly for
   your video file sizes.

   The presence of files that look like `*_overview.jpg`,
   `*_pageNNNN.jpg`, or `*_pageNNNN_NN_NN_NN.jpg` in a directory will
   make the :option:`-d, --directory <-d>` option think that thumbnails
   have already been generated for that directory and it will be
   skipped. Similarly, if the :option:`--subdir` option is a non-empty
   string then the presence of a sub-directory with that name will
   indicate that the parent directory should be skipped.

   :doc:`how-thumbnaildir` contains a detailed example of the use of the
   :option:`-d, --directory <-d>` option.

#. Why do I see black bars on the sides of my thumbnails?

   See the :option:`-p, --crop <-p>` option to manually set the cropping
   aspect ratio. :doc:`how-blackbars` contains a detailed example.

#. Why are my thumbnails squished/stretched?

   See the :option:`-t, --stretch <-t>` option to manually set the
   stretching aspect ratio. :doc:`how-distortion` contains a detailed
   example.

#. How do I change the location of the thumbnail timestamps?

   See the :option:`-l, --label <-l>` option to change the location of
   the timestamps from their default position in the lower-right corner.

#. How do I figure out the exact starting/ending thumbnailing times?

   Unfortunately, most video players don't seem to be able to jump to a
   particular time --- and when they are able to do this, they aren't
   able to do it while the video is paused.

   Luckily, |BSP|_ works pretty well for this:

   #. Pause the video.

   #. Type :kbd:`Control+J` to bring up the :guilabel:`Jump to time`
      dialog box or right-click on the video and choose
      :menuselection:`Playback --> Jump to time` from the popup menu.

   #. Enter in the time you want to jump to and press the :kbd:`Enter`
      key or click the :guilabel:`Jump` button.

   Alternatively, you can use Microsoft Expression Encoder:

   #. Import the video to view.

   #. Change the position using the text box below the timeline.

   Also, :mono:`MPEG2` (aka :mono:`.vob`) files sometimes have problems
   when determining video times. First of all make sure that the video
   codec used to generate the thumbnails is the same as that used by
   your player. If in doubt just use Microsoft Expression Encoder to
   view your video.

   Microsoft Expression Encoder doesn't seem to be able to accurately
   determine video positions when importing multiple :mono:`MPEG2`\ s
   into a single timeline (by using the :guilabel:`Insert a media item
   at the end of the timeline` button which is underneath the right side
   of the timeline). However, it will accurately show times within a
   single :mono:`MPEG2` so the workaround is to only add the final
   :mono:`MPEG2`/:mono:`.vob` and manually figure out the translation
   to/from single file time and total video time.

   |BSP|_ has a similar problem and again the workaround is to just view
   the final :mono:`MPEG2`/:mono:`.vob` instead of trying to jump to an
   actual time in the video.


Performance
===========

1. Why does it take so long to generate thumbnails?

   |CLATN| uses the |EESDK|_ to generate thumbnails and thus is dependent
   on its processing speed. In particular, the SDK can take some seconds
   to open the first video file.

   The upside is that the SDK is typically more accurate at grabbing
   thumbnails at a particular time and can grab thumbnails more
   frequently (down to every fraction of a second if desired). Other
   thumbnail tools can have difficulty grabbing frames more often than
   once every 15 or so seconds.

#. Why isn't there a nice fancy user interface for |CLATN|?

   |CLATN| is a **Command Line** program. However, it's been written to
   make it possible to bolt on a user interface at a later time since
   the command-line option processing is done separately from the
   actually thumbnail generation stuff. (Don't hold your breathe
   though).

#. Why doesn't |CLATN| use my fancy NVIDIA GPU to speed up thumbnail
   generation?

   The |EESDK|_ currently uses NVIDIA GPUs when encoding videos but not
   when decoding them.

#. Is |CLATN| able to use multiple CPU cores?

   No. If there ever is a GUI version of AutoThumbnailer then it will
   probably be able to use multiple cores. OTOH, you can open up
   multiple Command Prompt windows and run a separate instance of
   |CLATN| in each one.

..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
