.. default-role:: filename

.. _command-line-options:

================================
 Command-line Options Reference
================================

.. contents::
   :local:

Summary
=======

.. parsed-literal::

   Usage: CLAutoThumbnailer [OPTIONS] videofilename.ext |
                                      -d directory      |
                                      commandfile.txt
   Generates thumbnail pages for videos.

   Options:
     :option:`-d, --directory=DIRECTORY <-d>`  DIRECTORY to process. Generate thumbnails for
                                  files with the following extensions:
                                  3gp, asf, avi, divx, flv, m2v, m2p, m2ts, mkv,
                                  mov, mpeg, mpg, mp2, mp4, mts, ogm, rmvb, vob,
                                  wmv
         :option:`--exts=EXTENSIONS <--exts>`      add/remove video EXTENSIONS ("[+]ext1, -ext2")
         :option:`--minsize=FILESIZE <--minsize>`     Minimum FILESIZE of video files (0 to disable)
                                  [104857600 (100MB)]
     :option:`-m, --cmddir=DIRECTORY <-m>`     create initial command file for DIRECTORY
     :option:`-s, --start=TIME <-s>`           start TIME in h:mm:ss [0:00:05]
     :option:`-e, --end=TIME <-e>`             end TIME in h:mm:ss [-0:00:05]
     :option:`-v, --overview <-v>`            create Overview page (-v- disables) [True]
     :option:`-n=ROWS or COLUMNS <-n>`         Overview page desired # of ROWS or COLUMNS [12]
     :option:`-c, --columns=COLUMNS <-c>`      Overview page actual # of COLUMNS [12]
     :option:`-r, --rows=ROWS <-r>`            Overview page actual # of ROWS [12]
     :option:`-i, --interval=SECONDS <-i>`     Detail page thumbnail interval SECONDS [10.00]
         :option:`--autointerval`          use automatic interval based on duration [False]
         :option:`--autointervals=SPECIFICATION <--autointervals>`
                                automatic interval SPECIFICATION
                                  ( <min1=secs1, <min2=secs2, <min3=secs3, secs4 )
                                  [ <15=5, <30=10, <60=15, <90=30, 60 ]
     :option:`-N=ROWS or COLUMNS <-N>`         Detail page desired # of ROWS or COLUMNS [4]
     :option:`-C, --Columns=COLUMNS <-C>`      Detail page actual # of COLUMNS [4]
     :option:`-R, --Rows=ROWS <-R>`            Detail page actual # of ROWS [4]
         :option:`--dfts`                 add Detail page filename timestamps
                                  (--dfts- disables) [True]
     :option:`-y, --layout=MODE <-y>`          layout MODE
                                  (0=Auto,1=Actual,2=Row Priority,3=Column
                                  Priority) [Auto]
         :option:`--othres=THRESHOLD <--othres>`     video aspect ratio THRESHOLD for
                                  Auto Layout of Overview Page [0.00]
         :option:`--dthres=THRESHOLD <--dthres>`     video aspect ratio THRESHOLD for
                                  Auto Layout of Detail Pages [0.00]
         :option:`--rcopt`                do row/column optimizations
                                  (--rcopt- disables) [True]
         :option:`--maxoptsteps=STEPS <--maxoptsteps>`    max # of row/column optimization STEPS
                                  (0=unlimited) [2]
         :option:`--wthres=THRESHOLD <--wthres>`     width THRESHOLD for adding columns (0.1 - 1.0)
                                  [0.60]
         :option:`--hthres=THRESHOLD <--hthres>`     height THRESHOLD for adding rows (0.1 - 1.0)
                                  [0.60]
         :option:`--mincols=COLUMNS <--mincols>`      minimum # of COLUMNS [3]
         :option:`--minrows=ROWS <--minrows>`         minimum # of ROWS [3]
     :option:`-p, --crop=ASPECT RATIO <-p>`    crop ASPECT RATIO
         :option:`--rect=RECTANGLE <--rect>`       source RECTANGLE ( X,Y+WxH )
     :option:`-t, --stretch=ASPECT RATIO <-t>` stretch ASPECT RATIO
         :option:`--aar`                  do auto aspect ratio adjustment
                                  (--aar- disables) [True]
     :option:`-o, --outdir=DIRECTORY <-o>`     Output DIRECTORY
         :option:`--subdir=NAME <--subdir>`          Output sub-directory NAME [""]
         :option:`--name=NAME <--name>`            Display NAME
     :option:`-l, --label=POSITION <-l>`       timestamp label POSITION
                                  (0=Off,1=LR,2=LL,3=UR,4=UL) [LowerRight]
         :option:`--ms`                   show non-zero millisecond display in timestamps
                                  [False]
     :option:`-f, --scalefactor=SCALE FACTOR <-f>`
                                page SCALE FACTOR [1.00]
     :option:`-w, --width=PIXELS <-w>`         page width PIXELS [1280]
     :option:`-h, --height=PIXELS <-h>`        page height PIXELS [1024]
         :option:`--margin=PIXELS <--margin>`        margin between thumbnails PIXELS [2]
         :option:`--border=PIXELS <--border>`        thumbnail border width PIXELS [1]
         :option:`--save`                 save current settings as defaults
         :option:`--reset`                reset settings to initial defaults
         :option:`--dumpcr[=FILE] <--dumpcr>`        dump # columns/rows diagnostic plot to FILE
         :option:`--debug[=VALUE] <--debug>`       show debugging information
     :option:`-?, --help <--help>`                 show this message and exit
         :option:`--version`              show version and exit

In general, only the options with single letter abbreviations are
normally used and even most of those are for somewhat advanced
customizations.

.. _option-short-list:

Here's the list of options you'll most likely use:

   :option:`-?, --help <--help>`
      Show help message.

   :option:`-v, --overview <-v>`
      Control creation of Overview page.

   :option:`-s, --start <-s>`
      Set time to start thumbnailing.

   :option:`-e, --end <-e>`
      Set time to end thumbnailing.

   :option:`-n`
      Set Overview page desired number of rows or columns.

   :option:`-N`
      Set Detail page desired number of rows or columns.

   :option:`-i, --interval <-i>`
      Set Detail page thumbnail interval.

   :option:`-d, --directory <-d>`
      Generate thumbnails for a directory.

   :option:`-o, --outdir <-o>`
      Set output directory.

   :option:`-l, --label <-l>`
      Set timestamp label position.

   :option:`-m, --cmddir <-m>`
      Create initial command file for a directory.


"Boolean" options can be set to ``False`` by adding a ``-`` to the end
of the option. For example, to turn off the :option:`-v, --overview <-v>`
option you can specify either ``-v-`` or ``--overview-``.

To set a boolean option to ``True`` just specify the option or add a
``+`` to the end. For example, to turn on the :option:`--ms` option
specify either ``--ms`` or ``--ms+``.

|CLATN| option processing is done using the `NDesk.Options
<http://www.ndesk.org/Options>`_ "callback-based program option parser
for C#".


Input Options
=============

.. option:: -d <directory>, --directory <directory>

   Generate thumbnails for all video files in :optionarg:`directory` and
   its sub-directories. All files with recognized video extensions and
   that are greater than a minimum size will have thumbnails generated
   for them.

   By default, thumbnails are written to the original directory of each
   video file. The :option:`-o, --outdir <-o>` option will change this
   to a single directory (not recommended when generating Detail
   thumbnail pages for multiple directories).

   Use the :option:`-?, -h, --help <-h>` option to see the list of valid
   video extensions used during directory processing.

   If |CLATN| sees any files that match `*_overview.jpg`,
   `*_pageNNNN.jpg`, or `*_pageNNNN_NN_NN_NN.jpg` in a directory, it
   will be skipped since it is assumed that thumbnails have already been
   generated for it. You can trick the :option:`-d` option into skipping
   particular directories by putting dummy files in them with names like
   `skipthisdirectory_overview.jpg`.

   A directory will also be skipped if it already has a sub-directory
   with the same name as :option:`--subdir`.

   Checking for already created thumbnails only affects :option:`-d,
   --directory <-d>` option processing. If you explicitly list a file in
   the arguments to |CLATN| it will always be thumbnailed, regardless of
   the presence of previously generated thumbnails (which will be
   silently overwritten).

   While the best organization is to have one directory per movie, this
   isn't a requirement.

   Examples::

      clatn -d .

   will generate Overview and Detail thumbnail pages for all video files
   found in the current working directory and its sub-directories. The
   thumbnail pages will be placed in the original directories of each
   video file.

   ::

      clatn -d "C:\MyVideos\Latest"

   will generate Overview and Detail thumbnail pages for all video files
   found in `C:\\MyVideos\\Latest\\`. The thumbnail pages will be placed
   in the original directories of each video file.

   ::

      clatn -i 0 -o . -d "C:\MyVideos\Latest"

   will write only Overview thumbnail pages of all video files found in
   `C:\\MyVideos\\Latest\\` to the current working directory.

   See :doc:`how-thumbnaildir` for a detailed example.
   

.. option:: --exts <extensions>

   Add/remove video :optionarg:`extensions` that will be recognized
   during :option:`-d, --directory <-d>`
   processing. :optionarg:`Extensions` is a comma separated list of
   extensions. Prefix the extension with a ``-`` to remove it from the
   list, and with nothing or ``+`` to add it to the list.

   Examples::

      clatn --exts "abc, -mov" --directory .

   will add the extension ``abc`` and remove the extension ``mov`` while
   processing the current directory.

   ::

      clatn --exts "abc, -mov" --save

   will change |CLATN|’s default settings to add the extension ``abc``
   and remove the extension ``mov``.


.. option:: --minsize <bytes>

   Set the minimum size for video files during :option:`-d, --directory
   <-d>` processing to :optionarg:`bytes`.

   Range: :optionarg:`bytes` >= ``0``.

   Default is ``104857600 = 100 * 1024 * 1024`` (100MB).

   Examples::

      clatn --minsize=0 --directory .

   will allow files of any size while processing the current directory.

   ::

      clatn --minsize=0 --save

   will change |CLATN|’s default settings to allow files of any size
   during :option:`-d, --directory <-d>` processing.


.. option:: -m <directory>, --cmddir <directory>

   Create an initial command file called `CLAutotn-temp.txt` in the
   current working directory for :optionarg:`directory`.

   `CLAutotn-temp.txt` is just a text file that has an entry that looks
   like the following for each video found in :optionarg:`directory` and
   all its sub-directories::

      -i 0 -s 0:0:0 -e "reldir\videofilename.ext"

   Lines for files in directories that have already had thumbs generated
   for them will the be prefixed with a ``#`` character. It uses the
   same method that the :option:`-d, --directory <-d>` option uses to
   decide when to skip directories during processing.

   Example::

      clatn -m .

   will create a file called `CLAutotn-temp.txt` in the current
   directory that contains entries for all its video files.

   See :doc:`how-skipcreditscf` for a detailed example of how to create
   and use a Command File.


Time Range Options
==================

.. option:: -s <time>, --start <time>

   Set the start time to begin thumbnailing, where the :optionarg:`time`
   is :mono:`h:mm:ss.fff`. This is useful when you want to
   avoid wasting thumbnails on any boring opening credits.

   The default is :mono:`0:0:5`.

   Example::

      clatn -s 0:1:30 his_girl_friday_512kb.mp4

   will start thumbnailing 1 minute 30 seconds from the beginning of
   the video.

   See :doc:`how-skipcredits` for a detailed example.

.. option:: -e <time>, --end <time>

   Set the time to end thumbnailing, where the :optionarg:`time` is
   :mono:`h:mm:ss.fff`. This is useful when you want to avoid wasting
   thumbnails on the closing credits. A negative :optionarg:`time` means
   time from the end of the video.

   The default is :mono:`-0:0:5` (meaning create thumbnails until 5
   seconds from the end of the video).
   
   Example::

      clatn -e -0:2:05 his_girl_friday_512kb.mp4

   will stop thumbnailing at 2 minute 5 seconds from the end of
   the video.

   See :doc:`how-skipcredits` for a detailed example.


Overview Page Options
=====================

.. option:: -v, --overview

   Controls Overview page thumbnail generation. Add a ``-`` to turn off
   Overview page generation.

   Default is ``True``.

   Examples::

      clatn -v- his_girl_friday_512kb.mp4

   will skip generating the Overview thumbnail page for the current
   video.

   ::

      clatn --overview- --save

   will change |CLATN|’s default settings to normally skip Overview
   thumbnail page generation.


.. option:: -n <nRows or nColumns>

   Set the desired number of Overview page rows or columns to
   :optionarg:`nRows or nColumns`. Ignored when in :ref:`actual-layout` mode.

   The value specified is interpreted as the desired # of rows in
   :ref:`row-priority-layout` mode and the desired # of columns in
   :ref:`column-priority-layout` mode.

   In :ref:`auto-layout` mode, it is interpreted as the desired # of
   rows when the video's aspect ratio is greater than or equal to the
   thumbnail pages, otherwise it is treated as the desired # of columns.
   
   Notice that this option is lowercased to distinguish it from the
   :option:`-N` option.

   Range: :optionarg:`nRows or nColumns` >= ``1``.

   Default is ``12``.

   Example::

      clatn -n 10  his_girl_friday_512kb.mp4

   
.. option:: -c <nColumns>, --columns <nColumns>

   Set the actual number of Overview page columns to
   :option:`nColumns`. Only used when in :ref:`actual-layout` mode.

   Notice that this option is lowercased to distinguish it from the
   :option:`-C` option.

   Range: :optionarg:`nColumns` >= ``1``.

   Default is ``12``.


.. option:: -r <nRows>, --rows <nRows>

   Set the actual number of Overview page rows to :option:`nRows`. Only
   used when in :ref:`actual-layout`  mode.

   Notice that this option is lowercased to distinguish it from the
   :option:`-R` option.

   Range: :optionarg:`nRows` >= ``1``.

   Default is ``12``.


Detail Page Options
===================

.. option:: -i <interval>, --interval <interval>

   Set the Detail page time between thumbnails to :optionarg:`interval`
   where :optionarg:`interval` is :mono:`ss.fff`. Set
   :optionarg:`interval` to ``0`` to skip Detail page generation
   completely.

   Range: ``0`` or :optionarg:`interval` >= ``1 / 30`` (``0.0333``).

   Default is ``10``.

   Examples::

      clatn -i 60 his_girl_friday_512kb.mp4

   will generate a thumbnail every minute, while::

      clatn -i 0.20 his_girl_friday_512kb.mp4

   will generate 5 thumbnails per second. ::

      clatn -i 0 his_girl_friday_512kb.mp4

   will only generate an Overview thumbnail page and not generate any
   Detail thumbnail pages.

.. option:: --autointerval

   Controls setting the interval between thumbnails for Detail pages
   based on the thumbnailing duration.

   Default is ``False``.

   Here are the default auto-intervals:

   .. table::
      :class: centered, centercells

      +-----------------------------+--------------+
      |    Thumbnailing Duration    |   Interval   |                 
      +=============================+==============+
      |         <15 minutes         |   seconds    |
      +-----------------------------+--------------+
      |         <30 minutes         |  10 seconds  |
      +-----------------------------+--------------+
      |         <60 minutes         |  15 seconds  |
      +-----------------------------+--------------+
      |         <90 minutes         |  30 seconds  |
      +-----------------------------+--------------+
      |        >=90 minutes         |  60 seconds  |
      +-----------------------------+--------------+


.. option:: --autointervals <intervals>

   Set the automatic intervals to :optionarg:`intervals` where
   :optionarg:`intervals` is of the form ``<minutes1=seconds1,
   <minutes2=seconds2, <minutes3=seconds3, minutes4``. 

   Example::

      clatn --autointervals "<60=30, <120=60, 120" --directory .

   will change to the following auto-intervals while processing the
   current directory:

   .. table::
      :class: centered, centercells

      +-----------------------------+---------------+
      |    Thumbnailing Duration    |   Interval    |                 
      +=============================+===============+
      |         <60 minutes         |  30 seconds   |
      +-----------------------------+---------------+
      |        <120 minutes         |  60 seconds   |
      +-----------------------------+---------------+
      |        >=120 minutes        |  120 seconds  |
      +-----------------------------+---------------+

   ::

      clatn --autointervals "<60=30, <120=60, 120" --save

   will change |CLATN|’s default settings to those auto-intervals.

.. option:: -N <nRows or nColumns>

   Set the desired number of Detail page rows or columns to
   :optionarg:`nRows or nColumns`. Ignored when in :ref:`actual-layout`
   mode.

   The value specified is interpreted as the desired # of rows in
   :ref:`row-priority-layout` mode and the desired # of columns in
   :ref:`column-priority-layout` mode.

   In :ref:`auto-layout` mode, it is interpreted as the desired # of
   rows when the video's aspect ratio is greater than or equal to the
   thumbnail pages, otherwise it is treated as the desired # of columns.
   
   Setting :optionarg:`nRows or nColumns` to less than ``3`` is not
   generally recommended because :ref:`rc-optimization` can lead to
   results far from the desired number of rows or columns. In those
   situations either turn off Row/Column Optimization using
   :option:`--rcopt- <--rcopt>` or use the :option:`-y, --layout <-y>`
   option to use :ref:`actual-layout`. Also remember that you might need
   to change the minimum number of rows and columns using
   :option:`--mincols` and :option:`--minrows`.

   Notice that this option is Capitalized to distinguish it from the
   :option:`-n` option.

   Range: :optionarg:`nRows or nColumns` >= ``1``.

   Default is ``4``.

   Example::

      clatn -N 10  his_girl_friday_512kb.mp4


.. option:: -C <nColumns>, --Columns <nColumns>

   Set the actual number of Detail page columns to
   :option:`nColumns`. Only used when in :ref:`actual-layout` mode.

   Notice that this option is Capitalized to distinguish it from the
   :option:`-c` option.

   Range: :optionarg:`nColumns` >= ``1``.

   Default is ``4``.


.. option:: -R <nRows>, --Rows <nRows>

   Set the actual number of Detail page rows to :option:`nRows`. Only
   used when in :ref:`actual-layout` mode.

   Notice that this option is Capitalized to distinguish it from the
   :option:`-r` option.

   Range: :optionarg:`nRows` >= ``1``.

   Default is ``4``.


.. option:: --dfts

   Controls adding the timestamp of the first thumbnail on a Detail page
   to its filename. Normally the Detail thumbnail page filenames look
   like `displayfilename_pageNNNN_hh_mm_ss.jpg`, but specifying
   ``--dfts-`` will change it to just `displayfilename_pageNNNN.jpg`.

   Default is ``True``.


Layout Options
==============

.. option:: -y <mode>, --layout <mode>

   Set the :ref:`Page Layout Mode <page-layout-modes>` to
   :optionarg:`mode`, where :optionarg:`mode` is

      ``0`` =  :ref:`auto-layout`.

      ``1`` = :ref:`actual-layout`.

      ``2`` = :ref:`row-priority-layout`.

      ``3`` = :ref:`column-priority-layout`.


   :ref:`actual-layout` also uses :option:`-c, --columns <-c>` and
   :option:`-r, --rows <-r>` or :option:`-C, --Columns <-C>` and
   :option:`-R, --Rows <-r>` to determine the desired number of columns
   and rows.

   Default is ``0``, :ref:`auto-layout`.

   Example::

      clatn -l 1 -c 8 -r 9 -C 3 -R 4 his_girl_friday_512kb.mp4

   will force the Overview page to have 8 columns and 9 rows, and the
   Detail pages to have 3 columns and 4 rows.


.. option:: --othres <aspect ratio>

   In :ref:`auto-layout` of an Overview thumbnail page, :optionarg:`aspect
   ratio` to use when switching between :ref:`column-priority-layout`
   and :ref:`row-priority-layout`. ``0.0`` means use thumbnail page
   aspect ratio.

   Range: ``0.00`` or ``0.20`` <= :optionarg:`aspect ratio` <= ``4.0``.

   Default is ``0.0``.


.. option:: --dthres <aspect ratio>

   In :ref:`auto-layout` of Detail thumbnail pages, :optionarg:`aspect
   ratio` to use when switching between :ref:`column-priority-layout`
   and :ref:`row-priority-layout`. ``0.0`` means use thumbnail page
   aspect ratio.


   Range: ``0.00`` or ``0.20`` <= :optionarg:`aspect ratio` <= ``4.0``.

   Default is ``0.0``.


.. option:: --rcopt

   Controls :ref:`rc-optimization`.

   Default is ``True``.

   Examples::

      clatn --rcopt- his_girl_friday_512kb.mp4

   will disable row/column adjustment optimizations during the
   processing of the current video file.

   ::

      clatn --rcopt- --save

   will change |CLATN|’s default settings to normally disable row/column
   optimization.


.. option:: --maxoptsteps <steps>

   Set the maximum allowed number of :ref:`row/column optimization
   <rc-optimization>` steps to :optionarg:`step`. Set :optionarg:`step`
   to ``0`` to allow unlimited optimization steps. Use :option:`--rcopt`
   to completely disable :ref:`row/column optimization
   <rc-optimization>`.

   Range: :optionarg:`steps` >= ``0``.

   Default is ``2``.


.. option:: --wthres <threshold>

   Set the width threshold for adding columns to :optionarg:`theshold`
   where :optionarg:`theshold` is between ``0.1`` and ``1.0``. This is
   ignored if Row/Column Optimization is turned off via the
   :option:`--rcopt` option or the :ref:`row-priority-layout` is not
   being used.

   Range: ``0.1`` <= :optionarg:`threshold` <= ``1.0``.

   Default is ``0.60``.


.. option:: --hthres <threshold>

   Set the height threshold for adding rows to :optionarg:`theshold`
   where :optionarg:`theshold` is between ``0.1`` and ``1.0``. This is
   ignored if Row/Column Optimization is turned off via the
   :option:`--rcopt` option or the :ref:`column-priority-layout` is not
   being used.

   Range: ``0.1`` <= :optionarg:`threshold` <= ``1.0``.

   Default is ``0.60``.


.. option:: --mincols <nColumns>

   Set the minimum allowed number of columns to :optionarg:`nColumns`.

   Range: :optionarg:`nColumns` >= ``1``.

   Default is ``3``.


.. option:: --minrows <nRows>

   Set the minimum allowed number of rows to :optionarg:`nRows`.

   Range: :optionarg:`nRows` >= ``1``.

   Default is ``3``.


Aspect Ratio Options
====================

.. option:: -p <aspect ratio>, --crop <aspect ratio>

   Manually set the thumbnail cropping to :optionarg:`aspect ratio`. If
   you set the :term:`aspect ratio` smaller than the video frame's
   aspect ratio, you end up chopping off the sides of the video. If you
   set the aspect ratio larger, then you chop off the top and bottom.

   Automatically disables the :option:`--aar` option.

   Range: ``0.20`` <= :optionarg:`aspect ratio` <= ``4.0``.

   .. _letterbox-crop-values:

   In order to crop a letterboxed video you specify an aspect ratio to
   the :option:`-p, --crop <-p>` option that is :bi:`larger` than the
   video frame's actual aspect ratio. Common aspect ratios to specify
   would be the following (where you specify a slightly --- about 2% ---
   :bi:`smaller` value than the desired aspect ratio to
   leave a little bit of black to avoid overcropping):

   .. table:: Cropping letterboxed videos (black bars on top and bottom)
      :class: centered, centercells

      +---------------------+-----------------------+
      |Desired Aspect Ratio |  --crop Aspect Ratio  |
      +=====================+=======================+
      |        1.33         |        1.30           |
      +---------------------+-----------------------+
      |        1.78         |        1.74           |
      +---------------------+-----------------------+
      |        1.85         |        1.81           |
      +---------------------+-----------------------+
      |        2.35         |        2.30           |
      +---------------------+-----------------------+

   .. _pillarbox-crop-values:

   In order to crop a pillarboxed video you specify an aspect ratio to
   the :option:`-p, --crop <-p>` option that is :bi:`smaller` than the
   video frame's actual aspect ratio. Common aspect ratios to specify
   would be the following (where you specify a slightly --- about 2% ---
   :bi:`larger` value than the desired aspect ratio to leave a little
   bit of black to avoid overcropping):

   .. table:: Cropping pillarboxed videos (black bars on sides)
      :class: centered, centercells

      +---------------------+-----------------------+
      |Desired Aspect Ratio |  --crop Aspect Ratio  |
      +=====================+=======================+
      |        1.33         |        1.36           |
      +---------------------+-----------------------+
      |        1.78         |        1.81           |
      +---------------------+-----------------------+
      |        1.85         |        1.89           |
      +---------------------+-----------------------+
      |        2.35         |        2.40           |
      +---------------------+-----------------------+

   Example::

      clatn --crop 1.35 his_girl_friday.mpeg

   to leave a little bit more black border on the sides of the video
   than the automatically determined 1.33 aspect ratio.

   See :doc:`how-blackbars` for a detailed example.


.. option:: --rect <rectangle>

   Set the source thumbnailing :optionarg:`rectangle`, where
   :optionarg:`rectangle` is of the form ``X,Y+WIDTHxHEIGHT``.

   Automatically disables the :option:`--aar` option.

   See :ref:`cropping-windowboxed-videos` for a detailed example that
   uses the :option:`--rect` option.
   

.. option:: -t <aspect ratio>, --stretch <aspect ratio>

   Manually set the thumbnail stretching to :optionarg:`aspect ratio`.

   Automatically disables the :option:`--aar` option.

   Range: ``0.20`` <= :optionarg:`aspect ratio` <= ``4.0``.

   .. _common-aspect-ratios:

   In order to fix squished videos you specify an aspect ratio to the
   :option:`-t, --stretch <-t>` option that is :bi:`larger` (wider) than
   the video frame's actual aspect ratio.

   In order to fix stretched videos you specify an aspect ratio to the
   :option:`-t, --stretch <-t>` option that is :bi:`smaller` (narrower)
   than the video frame's actual aspect ratio.

   Common aspect ratios to try would be the following:

   .. table:: Common aspect ratios
      :class: centered, centercells

      +----------------+
      |  Aspect Ratio  |
      +================+
      |      1.33      |
      +----------------+
      |      1.78      |
      +----------------+
      |      1.85      |
      +----------------+
      |      2.35      |
      +----------------+

   Example::

      clatn --stretch 1.35 his_girl_friday_512kb.mp4

   See :doc:`how-distortion` for a detailed example that uses the
   :option:`-t, --stretch <-t>` option.


.. option:: --aar

   Controls automatic aspect ratio adjustment.

   Automatically disabled when the :option:`-p, --crop <-p>`,
   :option:`-t, --stretch <-t>`, or :option:`--rect` options are
   specified.

   Default is ``True`` (enabled).

   See :doc:`ref-aar` for more information.

Output Page Options
===================

.. option:: -o <directory>, --outdir <directory>

   Set the output directory for thumbnail pages to
   :optionarg:`directory`. :optionarg:`Directory` must already exist, it
   will :bi:`not` be created automatically. Normally you navigate to the
   directory where you want the thumbnails created and use ``-o .``.

   This option is required when the video file resides in a read-only
   directory (like on a DVD drive).

   In conjunction with the :option:`-d, --directory <-d>` and
   :option:`i, --interval <-i>` options, it can be used to summarize
   entire directories of videos by putting their Overview thumbnails in
   a single directory.

   Overrides the :option:`--subdir` option.

   Default is the original directory (or sub-directory if
   :option:`--subdir` is not an empty string) of each video file.

   Examples::

      clatn -o . E:\VIDEO_TS\VTS_01_*.VOB

   will write thumbnails of the first title set of the unencrypted DVD
   in drive `E:` to the current working directory .

   ::

      clatn -i 0 -o "C:\MyThumbnails" -d .

   will write only Overview thumbnails of all the video files in the
   current working directory (and all its sub-directories) to
   `C:\\MyThumbnails\\`.


.. option:: --subdir <name>

   Set the output directory for thumbnail pages to the sub-directory
   :optionarg:`name` of the original video directory. The
   :optionarg:`name` sub-directory will be created if it doesn't
   exist. :optionarg:`Name` can only contain alphanumerics, ``_``
   (underline), and ``-`` (minus) characters.

   Overridden by the :option:`-o, --outdir <-o>` option.

   Default is the empty string (use original directory of each video
   file for output).

   Example::

      clatn --subdir=Thumbs -d .

   will create thumbnails in a `Thumbs` subdir for each directory that
   contains videos. This is useful if you don't like mixing lots of
   thumbnail pages in with your video files.


.. option:: --name <displayname>

   Set the "display name" of the video file or files to
   :optionarg:`displayname`. The display name is put in the center of
   the thumbnail header and is also used to generate the output
   filenames.

   The default is the video filename, or in the case of multi-VOBs, the
   name of the parent directory + ``_-_vts_tt.dvd``, where ``tt`` is the
   DVD title number. Wildcard filenames like `videofilename_cd*.ext`
   become `videofilename_MULTI.ext`.


.. option:: -l <pos>, --label <pos>

   Set the position of the timestamp label to :optionarg:`pos`, where
   :optionarg:`pos` is:

      ``0`` = No timestamp.

      ``1`` = Timestamp in lower-right corner of thumbnails.

      ``2`` = Timestamp in lower-left corner of thumbnails.

      ``3`` = Timestamp in upper-right corner of thumbnails.

      ``4`` = Timestamp in upper-left corner of thumbnails.

   Default is ``1``.


.. option:: --ms

   Controls whether to always show non-zero milliseconds on timestamps.

   Non-zero milliseconds are always shown on Detail page timestamps of
   :doc:`ref-multipart`.

   Default is ``False``.


.. option:: -f <factor>, --scalefactor <factor>

   Set the scalefactor used to resize the thumbnail page to
   :optionarg:`factor`.

   Range: ``0.25`` <= :optionarg:`factor` <= ``3.0``.

   Default is ``1.0``.


.. option:: -w <pixels>, --width <pixels>

   Set the disired width of thumbnail pages to :optionarg:`pixels`.

   Range: :optionarg:`pixels` >= ``100``.

   Default is ``1280``.

.. option:: -h <pixels>, --height <pixels>

   Set the desired height of thumbnail pages to :optionarg:`pixels`.

   Range: :optionarg:`pixels` >= ``100``.

   Default is ``1024``.


.. option:: --margin <pixels>

   Set the width of the black space between thumbnails to
   :optionarg:`pixels`.

   Range: :optionarg:`pixels` >= ``0``.

   Default is ``2``.


.. option:: --border <pixels>

   Set the width of the white thumbnail border to :optionarg:`pixels`.

   Range: :optionarg:`pixels` >= ``0``.

   Default is ``1``.


Miscellaneous Options
=====================

.. option:: --save

   Save the current settings as |CLATN|’s new default settings.

   Any Help messages displayed from the same invocation that specified
   :option:`--save <--save>` will show the :bi:`previous` default
   settings. Run::

      clatn -?

   to see the newly saved defaults.

.. option:: --reset

   Reset |CLATN|’s default settings back to their initial values.


.. option:: --dumpcr [<filename>]

   For a range of video aspect ratios from 1.00 to 3.00, calculate the
   number of columns and rows that |CLATN| would use based on the
   thumbnail page's width (:option:`-w`) and height (:option:`-h`). It
   writes a plot of these results to :optionarg:`filename` or
   `colrowsplot.png` if :optionarg:`filename` is not specified.

   Other current settings that affect the calculation are: the desired
   number of rows or columns (:option:`-n` or :option:`-N`), layout mode
   (:option:`-y`), row/column optimization (:option:`--rcopt`), max
   optimization steps (:option:`--maxoptsteps`), Overview threshold
   (:option:`--othres`), Detail threshold (:option:`--dthres`), width
   threshold (:option:`--wthres`), height threshold
   (:option:`--hthres`), minimum columns (:option:`--mincols`), and
   minimum rows (:option:`--minrows`).

   By default it uses the Detail desired number of columns or rows
   (:option:`-N`). You can force it to use the Overview desired number
   of columns or rows (:option:`-n`) by specifying an interval
   (:option:`-i`) of ``0``.

   Prefixing the :optionarg:`filename` with ``nothreshold|`` will turn
   off the wasted width and height thresholding lines.

.. option:: --debug

   Display more verbose progress messages during operation.

.. option:: -?, --help

   Print a short description of all command line options.


.. option:: --version

   Print the |CLATN| version number and exit.



..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
