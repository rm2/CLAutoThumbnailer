.. default-role:: filename

==============
 Sample Files
==============

.. contents::
   :local:

His Girl Friday (1940)
======================

The files are organized in the following directory hierarchy::

   C:\His Girl Friday (1940)\
      mp4\
      mpeg2\
      pillarboxed\
      stretched\
      dvd\

Original Files
--------------

The main sample file used in the examples is the :mono:`MPEG4`, 382.6MB
version of His Girl Friday (1940) available at
http://www.archive.org/details/his_girl_friday.

.. _mediainfo-his_girl_friday_512kb-mp4:

`MediaInfo <http://mediainfo.sourceforge.net/en>`_ reports the following
for this file::

   General
   Complete name                    : C:\His Girl Friday (1940)\mp4\his_girl_friday_512kb.mp4
   Format                           : MPEG-4
   Format profile                   : Base Media
   Codec ID                         : isom
   File size                        : 383 MiB
   Duration                         : 1h 31mn
   Overall bit rate                 : 583 Kbps
   Movie name                       : His Girl Friday - http://www.archive.org/details/his_girl_friday
   Performer                        : Howard Hawks
   Screenplay by                    : Howard Hawks
   Encoded date                     : 1940
   Tagged date                      : UTC 2008-11-26 01:14:34
   Writing application              : Lavf51.10.0
   Comment                          : license:  http://creativecommons.org/licenses/publicdomain/

   Video
   ID                               : 1
   Format                           : AVC
   Format/Info                      : Advanced Video Codec
   Format profile                   : Baseline@L1.3
   Format settings, CABAC           : No
   Format settings, ReFrames        : 1 frame
   Codec ID                         : avc1
   Codec ID/Info                    : Advanced Video Coding
   Duration                         : 1h 31mn
   Bit rate mode                    : Variable
   Bit rate                         : 512 Kbps
   Width                            : 320 pixels
   Height                           : 240 pixels
   Display aspect ratio             : 4:3
   Frame rate mode                  : Constant
   Frame rate                       : 59.940 fps
   Color space                      : YUV
   Chroma subsampling               : 4:2:0
   Bit depth                        : 8 bits
   Scan type                        : Progressive
   Bits/(Pixel*Frame)               : 0.111
   Stream size                      : 336 MiB (88%)

   Audio
   ID                               : 2
   Format                           : AAC
   Format/Info                      : Advanced Audio Codec
   Format profile                   : LC
   Codec ID                         : 40
   Duration                         : 1h 31mn
   Bit rate mode                    : Variable
   Bit rate                         : 64.2 Kbps
   Channel(s)                       : 2 channels
   Channel positions                : Front: L R
   Sampling rate                    : 48.0 KHz
   Compression mode                 : Lossy
   Stream size                      : 42.1 MiB (11%)

.. _mediainfo-his_girl_friday-mpeg:

There is a much higher quality :mono:`MPEG2`, 3.1GB version of His Girl
Friday (1940) also available at
http://www.archive.org/details/his_girl_friday. It is used when an
example :mono:`MPEG2` video file is needed.

`MediaInfo <http://mediainfo.sourceforge.net/en>`_ reports the following
for the :mono:`MPEG2` file::

   General
   Complete name                    : C:\His Girl Friday (1940)\mpeg2\his_girl_friday.mpeg
   Format                           : MPEG-PS
   File size                        : 3.07 GiB
   Duration                         : 1h 31mn
   Overall bit rate                 : 4 785 Kbps

   Video
   ID                               : 224 (0xE0)
   Format                           : MPEG Video
   Format version                   : Version 2
   Format profile                   : Main@Main
   Format settings, BVOP            : Yes
   Format settings, Matrix          : Default
   Format settings, GOP             : M=3, N=12
   Duration                         : 1h 31mn
   Bit rate mode                    : Variable
   Bit rate                         : 4 497 Kbps
   Nominal bit rate                 : 2 950 Kbps
   Width                            : 720 pixels
   Height                           : 480 pixels
   Display aspect ratio             : 4:3
   Frame rate                       : 23.976 fps
   Standard                         : NTSC
   Color space                      : YUV
   Chroma subsampling               : 4:2:0
   Bit depth                        : 8 bits
   Scan type                        : Progressive
   Scan order                       : 2:3 Pulldown
   Compression mode                 : Lossy
   Bits/(Pixel*Frame)               : 0.543
   Stream size                      : 2.88 GiB (94%)

   Audio
   ID                               : 128 (0x80)
   Format                           : AC-3
   Format/Info                      : Audio Coding 3
   Mode extension                   : CM (complete main)
   Duration                         : 1h 31mn
   Bit rate mode                    : Constant
   Bit rate                         : 192 Kbps
   Channel(s)                       : 2 channels
   Channel positions                : Front: L R
   Sampling rate                    : 48.0 KHz
   Bit depth                        : 16 bits
   Compression mode                 : Lossy
   Delay relative to video          : -100ms
   Stream size                      : 126 MiB (4%)

Pillarboxed, Stretched, and Windowboxed Example Files
-----------------------------------------------------

Starting with `his_girl_friday_512kb.mp4`, I used the |EE| ``VC-1 512K
DSL VBR`` Preset with :guilabel:`Video Aspect Ratio` changed from
``Source`` to ``16:9`` and :guilabel:`Resize Mode` changed to
``Letterbox`` and ``Stretch`` to get the files used to demonstrate
:doc:`how-blackbars` and :doc:`how-distortion`.

`MediaInfo <http://mediainfo.sourceforge.net/en>`_ reports the following
for the pillarboxed file::

   General
   Complete name                    : C:\His Girl Friday (1940)\pillarboxed\his_girl_friday_512kb_pillarboxed.wmv
   Format                           : Windows Media
   File size                        : 334 MiB
   Duration                         : 1h 31mn
   Overall bit rate mode            : Variable
   Overall bit rate                 : 509 Kbps
   Maximum Overall bit rate         : 729 Kbps
   Encoded date                     : UTC 2011-05-25 00:08:26.562

   Video
   ID                               : 2
   Format                           : VC-1
   Format profile                   : AP@L1
   Codec ID                         : WVC1
   Codec ID/Hint                    : Microsoft
   Description of the codec         : VC-1 - VC-1 Advanced Profile
   Duration                         : 1h 31mn
   Bit rate mode                    : Variable
   Bit rate                         : 436 Kbps
   Width                            : 320 pixels
   Height                           : 180 pixels
   Display aspect ratio             : 16:9
   Frame rate                       : 59.940 fps
   Chroma subsampling               : 4:2:0
   Bit depth                        : 8 bits
   Scan type                        : Progressive
   Compression mode                 : Lossy
   Bits/(Pixel*Frame)               : 0.126
   Stream size                      : 286 MiB (86%)
   Language                         : English (US)

   Audio
   ID                               : 1
   Format                           : WMA
   Format profile                   : Pro
   Codec ID                         : 162
   Codec ID/Info                    : Windows Media Audio
   Description of the codec         : Windows Media Audio 10 Professional -  64 kbps, 44 kHz, 2 channel 16 bit 1-pass CBR
   Duration                         : 1h 31mn
   Bit rate mode                    : Constant
   Bit rate                         : 64.0 Kbps
   Channel(s)                       : 2 channels
   Sampling rate                    : 44.1 KHz
   Bit depth                        : 16 bits
   Stream size                      : 42.0 MiB (13%)
   Language                         : English (US)

`MediaInfo <http://mediainfo.sourceforge.net/en>`_ reports the following
for the stretched file::

   General
   Complete name                    : C:\His Girl Friday (1940)\stretched\his_girl_friday_512kb_stretched.wmv
   Format                           : Windows Media
   File size                        : 335 MiB
   Duration                         : 1h 31mn
   Overall bit rate mode            : Variable
   Overall bit rate                 : 511 Kbps
   Maximum Overall bit rate         : 729 Kbps
   Encoded date                     : UTC 2011-05-24 21:41:50.343

   Video
   ID                               : 2
   Format                           : VC-1
   Format profile                   : AP@L1
   Codec ID                         : WVC1
   Codec ID/Hint                    : Microsoft
   Description of the codec         : VC-1 - VC-1 Advanced Profile
   Duration                         : 1h 31mn
   Bit rate mode                    : Variable
   Bit rate                         : 436 Kbps
   Width                            : 320 pixels
   Height                           : 180 pixels
   Display aspect ratio             : 16:9
   Frame rate                       : 59.940 fps
   Chroma subsampling               : 4:2:0
   Bit depth                        : 8 bits
   Scan type                        : Progressive
   Compression mode                 : Lossy
   Bits/(Pixel*Frame)               : 0.126
   Stream size                      : 286 MiB (85%)
   Language                         : English (US)

   Audio
   ID                               : 1
   Format                           : WMA
   Format profile                   : Pro
   Codec ID                         : 162
   Codec ID/Info                    : Windows Media Audio
   Description of the codec         : Windows Media Audio 10 Professional -  64 kbps, 44 kHz, 2 channel 16 bit 1-pass CBR
   Duration                         : 1h 31mn
   Bit rate mode                    : Constant
   Bit rate                         : 64.0 Kbps
   Channel(s)                       : 2 channels
   Sampling rate                    : 44.1 KHz
   Bit depth                        : 16 bits
   Stream size                      : 42.0 MiB (13%)
   Language                         : English (US)


I used the |EE| ``VC-1 512K DSL VBR`` Preset with :guilabel:`Video
Aspect Ratio` changed from ``Source`` to ``4:3`` and :guilabel:`Resize
Mode` changed to ``Letterbox`` on
``his_girl_friday_512kb_pillarboxed.wmv`` to get the file used to
demonstrate :ref:`cropping-windowboxed-videos`.

`MediaInfo <http://mediainfo.sourceforge.net/en>`_ reports the following
for the windowboxed file::

   General
   Complete name                    : C:\His Girl Friday (1940)\windowboxed\his_girl_friday_512kb_windowboxed.wmv
   Format                           : Windows Media
   File size                        : 334 MiB
   Duration                         : 1h 31mn
   Overall bit rate mode            : Variable
   Overall bit rate                 : 509 Kbps
   Maximum Overall bit rate         : 729 Kbps
   Encoded date                     : UTC 2011-05-30 15:03:26.737

   Video
   ID                               : 2
   Format                           : VC-1
   Format profile                   : AP@L1
   Codec ID                         : WVC1
   Codec ID/Hint                    : Microsoft
   Duration                         : 1h 31mn
   Bit rate mode                    : Variable
   Bit rate                         : 436 Kbps
   Width                            : 320 pixels
   Height                           : 240 pixels
   Display aspect ratio             : 4:3
   Frame rate                       : 59.940 fps
   Chroma subsampling               : 4:2:0
   Bit depth                        : 8 bits
   Scan type                        : Progressive
   Compression mode                 : Lossy
   Bits/(Pixel*Frame)               : 0.095
   Stream size                      : 286 MiB (86%)
   Language                         : English (US)

   Audio
   ID                               : 1
   Format                           : WMA
   Format profile                   : Pro
   Codec ID                         : 162
   Codec ID/Info                    : Windows Media Audio
   Duration                         : 1h 31mn
   Bit rate mode                    : Constant
   Bit rate                         : 64.0 Kbps
   Channel(s)                       : 2 channels
   Sampling rate                    : 44.1 KHz
   Bit depth                        : 16 bits
   Stream size                      : 42.0 MiB (13%)
   Language                         : English (US)


.. _sample-dvd:

DVD Files
---------

In order to demonstrate :doc:`how-dvd`, I used `DVDStyler
<http://www.dvdstyler.de/en/>`_ on `his_girl_friday.mpeg` to create a
single title DVD. It created the following files in a `VIDEO_TS`
directory::

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

`MediaInfo <http://mediainfo.sourceforge.net/en>`_ reports the following
for the title set files::

   General
   Complete name                    : C:\His Girl Friday (1940)\dvd\VIDEO_TS\VTS_01_1.VOB
   Format                           : MPEG-PS
   File size                        : 1 024 MiB
   Duration                         : 30mn 10s
   Overall bit rate                 : 4 745 Kbps

   Video
   ID                               : 224 (0xE0)
   Format                           : MPEG Video
   Format version                   : Version 2
   Format profile                   : Main@Main
   Format settings, BVOP            : Yes
   Format settings, Matrix          : Default
   Format settings, GOP             : M=3, N=12
   Duration                         : 30mn 10s
   Bit rate mode                    : Variable
   Bit rate                         : 4 458 Kbps
   Nominal bit rate                 : 2 950 Kbps
   Width                            : 720 pixels
   Height                           : 480 pixels
   Display aspect ratio             : 4:3
   Frame rate                       : 23.976 fps
   Standard                         : NTSC
   Color space                      : YUV
   Chroma subsampling               : 4:2:0
   Bit depth                        : 8 bits
   Scan type                        : Progressive
   Scan order                       : 2:3 Pulldown
   Compression mode                 : Lossy
   Bits/(Pixel*Frame)               : 0.538
   Stream size                      : 962 MiB (94%)

   Audio
   ID                               : 128 (0x80)
   Format                           : AC-3
   Format/Info                      : Audio Coding 3
   Mode extension                   : CM (complete main)
   Duration                         : 30mn 10s
   Bit rate mode                    : Constant
   Bit rate                         : 192 Kbps
   Channel(s)                       : 2 channels
   Channel positions                : Front: L R
   Sampling rate                    : 48.0 KHz
   Bit depth                        : 16 bits
   Compression mode                 : Lossy
   Delay relative to video          : -100ms
   Stream size                      : 41.4 MiB (4%)

   Menu

::

   General
   Complete name                    : C:\His Girl Friday (1940)\dvd\VIDEO_TS\VTS_01_2.VOB
   Format                           : MPEG-PS
   File size                        : 1 024 MiB
   Duration                         : 30mn 13s
   Overall bit rate                 : 4 735 Kbps

   Video
   ID                               : 224 (0xE0)
   Format                           : MPEG Video
   Format version                   : Version 2
   Format profile                   : Main@Main
   Format settings, BVOP            : Yes
   Format settings, Matrix          : Default
   Format settings, GOP             : M=3, N=12
   Duration                         : 30mn 13s
   Bit rate mode                    : Variable
   Bit rate                         : 4 449 Kbps
   Width                            : 720 pixels
   Height                           : 480 pixels
   Display aspect ratio             : 4:3
   Frame rate                       : 23.976 fps
   Standard                         : NTSC
   Color space                      : YUV
   Chroma subsampling               : 4:2:0
   Bit depth                        : 8 bits
   Scan type                        : Progressive
   Scan order                       : 2:3 Pulldown
   Compression mode                 : Lossy
   Bits/(Pixel*Frame)               : 0.537
   Stream size                      : 962 MiB (94%)

   Audio
   ID                               : 128 (0x80)
   Format                           : AC-3
   Format/Info                      : Audio Coding 3
   Mode extension                   : CM (complete main)
   Duration                         : 30mn 13s
   Bit rate mode                    : Constant
   Bit rate                         : 192 Kbps
   Channel(s)                       : 2 channels
   Channel positions                : Front: L R
   Sampling rate                    : 48.0 KHz
   Bit depth                        : 16 bits
   Compression mode                 : Lossy
   Delay relative to video          : -398ms
   Stream size                      : 41.5 MiB (4%)

   Menu

::

   General
   Complete name                    : C:\His Girl Friday (1940)\dvd\VIDEO_TS\VTS_01_3.VOB
   Format                           : MPEG-PS
   File size                        : 1 024 MiB
   Duration                         : 30mn 10s
   Overall bit rate                 : 4 745 Kbps

   Video
   ID                               : 224 (0xE0)
   Format                           : MPEG Video
   Format version                   : Version 2
   Format profile                   : Main@Main
   Format settings, BVOP            : Yes
   Format settings, Matrix          : Default
   Format settings, GOP             : M=3, N=12
   Duration                         : 30mn 10s
   Bit rate mode                    : Variable
   Bit rate                         : 4 459 Kbps
   Width                            : 720 pixels
   Height                           : 480 pixels
   Display aspect ratio             : 4:3
   Frame rate                       : 23.976 fps
   Standard                         : NTSC
   Color space                      : YUV
   Chroma subsampling               : 4:2:0
   Bit depth                        : 8 bits
   Scan type                        : Progressive
   Scan order                       : 2:3 Pulldown
   Compression mode                 : Lossy
   Bits/(Pixel*Frame)               : 0.538
   Stream size                      : 962 MiB (94%)

   Audio
   ID                               : 128 (0x80)
   Format                           : AC-3
   Format/Info                      : Audio Coding 3
   Mode extension                   : CM (complete main)
   Duration                         : 30mn 10s
   Bit rate mode                    : Constant
   Bit rate                         : 192 Kbps
   Channel(s)                       : 2 channels
   Channel positions                : Front: L R
   Sampling rate                    : 48.0 KHz
   Bit depth                        : 16 bits
   Compression mode                 : Lossy
   Delay relative to video          : -240ms
   Stream size                      : 41.4 MiB (4%)

   Menu

::

   General
   Complete name                    : C:\His Girl Friday (1940)\dvd\VIDEO_TS\VTS_01_4.VOB
   Format                           : MPEG-PS
   File size                        : 46.5 MiB
   Duration                         : 1mn 9s
   Overall bit rate                 : 5 583 Kbps

   Video
   ID                               : 224 (0xE0)
   Format                           : MPEG Video
   Format version                   : Version 2
   Format profile                   : Main@Main
   Format settings, BVOP            : Yes
   Format settings, Matrix          : Default
   Format settings, GOP             : M=3, N=12
   Duration                         : 1mn 9s
   Bit rate mode                    : Variable
   Bit rate                         : 5 279 Kbps
   Width                            : 720 pixels
   Height                           : 480 pixels
   Display aspect ratio             : 4:3
   Frame rate                       : 23.976 fps
   Standard                         : NTSC
   Color space                      : YUV
   Chroma subsampling               : 4:2:0
   Bit depth                        : 8 bits
   Scan type                        : Progressive
   Scan order                       : 2:3 Pulldown
   Compression mode                 : Lossy
   Bits/(Pixel*Frame)               : 0.637
   Stream size                      : 43.6 MiB (94%)

   Audio
   ID                               : 128 (0x80)
   Format                           : AC-3
   Format/Info                      : Audio Coding 3
   Mode extension                   : CM (complete main)
   Duration                         : 1mn 9s
   Bit rate mode                    : Constant
   Bit rate                         : 192 Kbps
   Channel(s)                       : 2 channels
   Channel positions                : Front: L R
   Sampling rate                    : 48.0 KHz
   Bit depth                        : 16 bits
   Compression mode                 : Lossy
   Delay relative to video          : -325ms
   Stream size                      : 1.60 MiB (3%)

   Menu


McLintock! (1963)
=================

The files are organized in the following directory hierarchy::

   C:\McLintock (1963)\
      mp4 512kb\
      mp4\
      letterboxed\
      squished\
      multipart\

Original Files
--------------

The main widescreen sample file used in the examples is the 512Kbps
:mono:`MPEG4`, 525.3MB version of McLintock! (1963) available at
http://www.archive.org/details/mclintok_widescreen.

`MediaInfo <http://mediainfo.sourceforge.net/en>`_ reports the following
for this file::

   General
   Complete name                    : C:\McLintock (1963)\mp4 512kb\McLintock_512kb.mp4
   Format                           : MPEG-4
   Format profile                   : Base Media
   Codec ID                         : isom
   File size                        : 525 MiB
   Duration                         : 2h 6mn
   Overall bit rate                 : 579 Kbps
   Movie name                       : McLintok! - http://www.archive.org/details/mclintok_widescreen
   Encoded date                     : 1963
   Tagged date                      : UTC 2008-11-27 14:59:27
   Writing application              : Lavf51.10.0
   Comment                          : license:  http://creativecommons.org/licenses/publicdomain/

   Video
   ID                               : 1
   Format                           : AVC
   Format/Info                      : Advanced Video Codec
   Format profile                   : Baseline@L1.3
   Format settings, CABAC           : No
   Format settings, ReFrames        : 1 frame
   Codec ID                         : avc1
   Codec ID/Info                    : Advanced Video Coding
   Duration                         : 2h 6mn
   Bit rate mode                    : Variable
   Bit rate                         : 512 Kbps
   Width                            : 556 pixels
   Height                           : 240 pixels
   Display aspect ratio             : 2.35:1
   Frame rate mode                  : Constant
   Frame rate                       : 23.976 fps
   Color space                      : YUV
   Chroma subsampling               : 4:2:0
   Bit depth                        : 8 bits
   Scan type                        : Progressive
   Bits/(Pixel*Frame)               : 0.160
   Stream size                      : 464 MiB (88%)

   Audio
   ID                               : 2
   Format                           : AAC
   Format/Info                      : Advanced Audio Codec
   Format profile                   : LC
   Codec ID                         : 40
   Duration                         : 2h 6mn
   Bit rate mode                    : Variable
   Bit rate                         : 62.9 Kbps
   Channel(s)                       : 2 channels
   Channel positions                : Front: L R
   Sampling rate                    : 44.1 KHz
   Compression mode                 : Lossy
   Stream size                      : 57.1 MiB (11%)


There is also a nicer quality 1500 Kbps :mono:`MPEG4`, 1.4G version of
McLintock!  (1963) available at
http://www.archive.org/details/mclintok_widescreen.

`MediaInfo <http://mediainfo.sourceforge.net/en>`_ reports the following
for the 1500 Kbps :mono:`MPEG4` file::

   General
   Complete name                    : C:\McLintock (1963)\mp4\McLintock.mp4
   Format                           : MPEG-4
   Codec ID                         : M4V 
   File size                        : 1.45 GiB
   Duration                         : 2h 6mn
   Overall bit rate                 : 1 634 Kbps
   Encoded date                     : UTC 2007-01-25 05:44:03
   Tagged date                      : UTC 2007-01-25 05:46:15

   Video
   ID                               : 2
   Format                           : AVC
   Format/Info                      : Advanced Video Codec
   Format profile                   : Baseline@L2.1
   Format settings, CABAC           : No
   Format settings, ReFrames        : 1 frame
   Codec ID                         : avc1
   Codec ID/Info                    : Advanced Video Coding
   Duration                         : 2h 6mn
   Bit rate mode                    : Variable
   Bit rate                         : 1 501 Kbps
   Maximum bit rate                 : 4 006 Kbps
   Width                            : 640 pixels
   Height                           : 276 pixels
   Display aspect ratio             : 2.35:1
   Frame rate mode                  : Variable
   Frame rate                       : 23.976 fps
   Minimum frame rate               : 23.967 fps
   Maximum frame rate               : 30.000 fps
   Color space                      : YUV
   Chroma subsampling               : 4:2:0
   Bit depth                        : 8 bits
   Scan type                        : Progressive
   Bits/(Pixel*Frame)               : 0.355
   Stream size                      : 1.33 GiB (92%)
   Language                         : English
   Encoded date                     : UTC 2007-01-25 02:41:01
   Tagged date                      : UTC 2007-01-25 05:46:15
   Color primaries                  : BT.601-6 525, BT.1358 525, BT.1700 NTSC, SMPTE 170M
   Transfer characteristics         : BT.709-5, BT.1361
   Matrix coefficients              : BT.601-6 525, BT.1358 525, BT.1700 NTSC, SMPTE 170M

   Audio
   ID                               : 1
   Format                           : AAC
   Format/Info                      : Advanced Audio Codec
   Format profile                   : LC
   Codec ID                         : 40
   Duration                         : 2h 6mn
   Bit rate mode                    : Constant
   Bit rate                         : 128 Kbps
   Channel(s)                       : 2 channels
   Channel positions                : Front: L R
   Sampling rate                    : 44.1 KHz
   Compression mode                 : Lossy
   Stream size                      : 116 MiB (8%)
   Language                         : English
   Encoded date                     : UTC 2007-01-25 02:40:58
   Tagged date                      : UTC 2007-01-25 05:46:15


Letterboxed and Squished Example Files
---------------------------------------

Starting with `McLintock_512kb.mp4`, I used the |EE| ``VC-1 512K DSL
VBR`` Preset with :guilabel:`Video Aspect Ratio` changed from ``Source``
to ``4:3`` and :guilabel:`Resize Mode` changed to ``Letterbox`` and
``Stretch`` to get the files used to demonstrate :doc:`how-blackbars`
and :doc:`how-distortion`.

`MediaInfo <http://mediainfo.sourceforge.net/en>`_ reports the following
for the letterboxed file::

   General
   Complete name                    : C:\McLintock (1963)\letterboxed\McLintock_512kb_letterboxed.wmv
   Format                           : Windows Media
   File size                        : 439 MiB
   Duration                         : 2h 6mn
   Overall bit rate mode            : Variable
   Overall bit rate                 : 484 Kbps
   Maximum Overall bit rate         : 724 Kbps
   Movie name                       : McLintok! - http://www.archive.org/details/mclintok_widescreen
   Recorded date                    : 1963
   Encoded date                     : UTC 2011-05-25 11:24:46.328
   Comment                          : license:  http://creativecommons.org/licenses/publicdomain/

   Video
   ID                               : 2
   Format                           : VC-1
   Format profile                   : AP@L0
   Codec ID                         : WVC1
   Codec ID/Hint                    : Microsoft
   Description of the codec         : VC-1 - VC-1 Advanced Profile
   Duration                         : 2h 6mn
   Bit rate mode                    : Variable
   Bit rate                         : 436 Kbps
   Width                            : 320 pixels
   Height                           : 240 pixels
   Display aspect ratio             : 4:3
   Frame rate                       : 23.976 fps
   Chroma subsampling               : 4:2:0
   Bit depth                        : 8 bits
   Scan type                        : Progressive
   Compression mode                 : Lossy
   Bits/(Pixel*Frame)               : 0.237
   Stream size                      : 395 MiB (90%)

   Audio
   ID                               : 1
   Format                           : WMA
   Format profile                   : Pro
   Codec ID                         : 162
   Codec ID/Info                    : Windows Media Audio
   Description of the codec         : Windows Media Audio 10 Professional -  64 kbps, 44 kHz, 2 channel 16 bit 1-pass CBR
   Duration                         : 2h 6mn
   Bit rate mode                    : Constant
   Bit rate                         : 64.0 Kbps
   Channel(s)                       : 2 channels
   Sampling rate                    : 44.1 KHz
   Bit depth                        : 16 bits
   Stream size                      : 58.1 MiB (13%)

`MediaInfo <http://mediainfo.sourceforge.net/en>`_ reports the following
for the squished file::

   General
   Complete name                    : C:\McLintock (1963)\squished\McLintock_512kb_squished.wmv
   Format                           : Windows Media
   File size                        : 441 MiB
   Duration                         : 2h 6mn
   Overall bit rate mode            : Variable
   Overall bit rate                 : 486 Kbps
   Maximum Overall bit rate         : 724 Kbps
   Movie name                       : McLintok! - http://www.archive.org/details/mclintok_widescreen
   Recorded date                    : 1963
   Encoded date                     : UTC 2011-05-25 14:03:17.921
   Comment                          : license:  http://creativecommons.org/licenses/publicdomain/

   Video
   ID                               : 2
   Format                           : VC-1
   Format profile                   : AP@L0
   Codec ID                         : WVC1
   Codec ID/Hint                    : Microsoft
   Description of the codec         : VC-1 - VC-1 Advanced Profile
   Duration                         : 2h 6mn
   Bit rate mode                    : Variable
   Bit rate                         : 436 Kbps
   Width                            : 320 pixels
   Height                           : 240 pixels
   Display aspect ratio             : 4:3
   Frame rate                       : 23.976 fps
   Chroma subsampling               : 4:2:0
   Bit depth                        : 8 bits
   Scan type                        : Progressive
   Compression mode                 : Lossy
   Bits/(Pixel*Frame)               : 0.237
   Stream size                      : 395 MiB (90%)

   Audio
   ID                               : 1
   Format                           : WMA
   Format profile                   : Pro
   Codec ID                         : 162
   Codec ID/Info                    : Windows Media Audio
   Description of the codec         : Windows Media Audio 10 Professional -  64 kbps, 44 kHz, 2 channel 16 bit 1-pass CBR
   Duration                         : 2h 6mn
   Bit rate mode                    : Constant
   Bit rate                         : 64.0 Kbps
   Channel(s)                       : 2 channels
   Sampling rate                    : 44.1 KHz
   Bit depth                        : 16 bits
   Stream size                      : 58.1 MiB (13%)


.. _sample-multi-part:

Multi-Part Video Example Files
------------------------------

In order to demonstrate :doc:`how-multipart`, I split `McLintock.mp4`
into three equal sized pieces.

`MediaInfo <http://mediainfo.sourceforge.net/en>`_ reports the following
for these files::

   General
   Complete name                    : C:\McLintock (1963)\multipart\McLintock_part01.mp4
   Format                           : MPEG-4
   Format profile                   : Base Media
   Codec ID                         : isom
   File size                        : 541 MiB
   Duration                         : 42mn 16s
   Overall bit rate                 : 1 791 Kbps
   Encoded date                     : UTC 2011-05-25 20:30:42
   Tagged date                      : UTC 2011-05-25 20:30:42
   Writing application              : Lavf51.12.1

   Video
   ID                               : 1
   Format                           : AVC
   Format/Info                      : Advanced Video Codec
   Format profile                   : Baseline@L5.1
   Format settings, CABAC           : No
   Format settings, ReFrames        : 1 frame
   Codec ID                         : avc1
   Codec ID/Info                    : Advanced Video Coding
   Duration                         : 42mn 16s
   Bit rate mode                    : Variable
   Bit rate                         : 1 658 Kbps
   Width                            : 640 pixels
   Height                           : 276 pixels
   Display aspect ratio             : 2.35:1
   Frame rate mode                  : Constant
   Frame rate                       : 23.976 fps
   Color space                      : YUV
   Chroma subsampling               : 4:2:0
   Bit depth                        : 8 bits
   Scan type                        : Progressive
   Bits/(Pixel*Frame)               : 0.392
   Stream size                      : 501 MiB (93%)
   Language                         : English
   Encoded date                     : UTC 2011-05-25 20:30:42
   Tagged date                      : UTC 2011-05-25 20:30:42

   Audio
   ID                               : 2
   Format                           : AAC
   Format/Info                      : Advanced Audio Codec
   Format profile                   : LC
   Codec ID                         : 40
   Duration                         : 42mn 16s
   Bit rate mode                    : Variable
   Bit rate                         : 128 Kbps
   Channel(s)                       : 2 channels
   Channel positions                : Front: L R
   Sampling rate                    : 44.1 KHz
   Compression mode                 : Lossy
   Stream size                      : 38.7 MiB (7%)
   Language                         : English
   Encoded date                     : UTC 2011-05-25 20:30:42
   Tagged date                      : UTC 2011-05-25 20:30:42

::

   General
   Complete name                    : C:\McLintock (1963)\multipart\McLintock_part02.mp4
   Format                           : MPEG-4
   Format profile                   : Base Media
   Codec ID                         : isom
   File size                        : 543 MiB
   Duration                         : 42mn 16s
   Overall bit rate                 : 1 796 Kbps
   Encoded date                     : UTC 2011-05-25 20:54:12
   Tagged date                      : UTC 2011-05-25 20:54:12
   Writing application              : Lavf51.12.1

   Video
   ID                               : 1
   Format                           : AVC
   Format/Info                      : Advanced Video Codec
   Format profile                   : Baseline@L5.1
   Format settings, CABAC           : No
   Format settings, ReFrames        : 1 frame
   Codec ID                         : avc1
   Codec ID/Info                    : Advanced Video Coding
   Duration                         : 42mn 16s
   Bit rate mode                    : Variable
   Bit rate                         : 1 669 Kbps
   Width                            : 640 pixels
   Height                           : 276 pixels
   Display aspect ratio             : 2.35:1
   Frame rate mode                  : Constant
   Frame rate                       : 23.976 fps
   Color space                      : YUV
   Chroma subsampling               : 4:2:0
   Bit depth                        : 8 bits
   Scan type                        : Progressive
   Bits/(Pixel*Frame)               : 0.394
   Stream size                      : 504 MiB (93%)
   Language                         : English
   Encoded date                     : UTC 2011-05-25 20:54:12
   Tagged date                      : UTC 2011-05-25 20:54:12

   Audio
   ID                               : 2
   Format                           : AAC
   Format/Info                      : Advanced Audio Codec
   Format profile                   : LC
   Codec ID                         : 40
   Duration                         : 42mn 16s
   Bit rate mode                    : Variable
   Bit rate                         : 123 Kbps
   Channel(s)                       : 2 channels
   Channel positions                : Front: L R
   Sampling rate                    : 44.1 KHz
   Compression mode                 : Lossy
   Stream size                      : 37.1 MiB (7%)
   Language                         : English
   Encoded date                     : UTC 2011-05-25 20:54:12
   Tagged date                      : UTC 2011-05-25 20:54:12

::

   General
   Complete name                    : C:\McLintock (1963)\multipart\McLintock_part03.mp4
   Format                           : MPEG-4
   Format profile                   : Base Media
   Codec ID                         : isom
   File size                        : 545 MiB
   Duration                         : 42mn 15s
   Overall bit rate                 : 1 802 Kbps
   Encoded date                     : UTC 2011-05-25 21:18:05
   Tagged date                      : UTC 2011-05-25 21:18:05
   Writing application              : Lavf51.12.1

   Video
   ID                               : 1
   Format                           : AVC
   Format/Info                      : Advanced Video Codec
   Format profile                   : Baseline@L5.1
   Format settings, CABAC           : No
   Format settings, ReFrames        : 1 frame
   Codec ID                         : avc1
   Codec ID/Info                    : Advanced Video Coding
   Duration                         : 42mn 15s
   Bit rate mode                    : Variable
   Bit rate                         : 1 681 Kbps
   Width                            : 640 pixels
   Height                           : 276 pixels
   Display aspect ratio             : 2.35:1
   Frame rate mode                  : Constant
   Frame rate                       : 23.976 fps
   Color space                      : YUV
   Chroma subsampling               : 4:2:0
   Bit depth                        : 8 bits
   Scan type                        : Progressive
   Bits/(Pixel*Frame)               : 0.397
   Stream size                      : 508 MiB (93%)
   Language                         : English
   Encoded date                     : UTC 2011-05-25 21:18:05
   Tagged date                      : UTC 2011-05-25 21:18:05

   Audio
   ID                               : 2
   Format                           : AAC
   Format/Info                      : Advanced Audio Codec
   Format profile                   : LC
   Codec ID                         : 40
   Duration                         : 42mn 14s
   Bit rate mode                    : Variable
   Bit rate                         : 117 Kbps
   Channel(s)                       : 2 channels
   Channel positions                : Front: L R
   Sampling rate                    : 44.1 KHz
   Compression mode                 : Lossy
   Stream size                      : 35.4 MiB (7%)
   Language                         : English
   Encoded date                     : UTC 2011-05-25 21:18:05
   Tagged date                      : UTC 2011-05-25 21:18:05


..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
