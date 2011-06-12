.. default-role:: filename

=================
 Troubleshooting
=================

.. contents::
   :local:

.. _dealing-with-codecs:

Dealing with Codecs
===================

Understanding Codec Basics
--------------------------

In order to play a video (or make thumbnails using |CLATN|) you need at
least three additional pieces of software:

#. A "splitter" (aka `demultiplexer
   <http://en.wikipedia.org/wiki/Demultiplexer_(media_file)>`_).

#. A video :term:`codec`.

#. An audio codec.

The "splitter" splits the video file into separate audio and video
streams that the other two codecs can use. I recommend using the either
the splitter that comes with |EE| or the `Haali Media Splitter
<http://haali.su/mkv/>`_.

For the video and audio codecs, I recommend either the codecs
specifically shipped with |EE| and whose names start with "Expression
Encoder" or the :ref:`ffdshow codec <ffdshow-codec>`.

Unfortunately, just because you can play a video doesn't mean that you
can generate thumbnails for it. This is because |EE| maintains a
separate list of enabled codecs (which it calls "filters") that also has
to be set correctly. This is explained in more detail in the next
section.


.. _ee-video-filters:

|EE|’s Video Filter List
------------------------

Even though |CLATN| is a command line program, it is written using the
|EESDK|_ so it also depends on the |EE| user interface. In particular,
|CLATN| uses the exact same enabled video filter set that |EE| is
currently using.

See `Troubleshoot playback problems
<http://expression.microsoft.com/en-us/library/cc294533>`_ for
instructions on how to enable and disable video filters (aka video
:term:`codecs <codec>`). If |EE| can't import a video then |CLATN| will
also be unable to generate thumbnails for that video, and you'll see the
"isn't a video file" error message. Any tips you find online for
importing videos for |EE| will also apply to |CLATN|.

When you first install |EE| it enables all the video filters that it
finds on your system. For tracking down problems however, it's much
better to use only a very limited set of filters:

.. figure:: images/ee4ffdshowfilters.png
   :align: center
   :alt: Recommended ffdshow filter set

   Recommended filter set when using ffdshow codecs

.. figure:: images/ee4msfilters.png
   :align: center
   :alt: Recommended |EE| filter set

   Recommended filter set when using |EE| codecs

If you need to support :mono:`MPEG` video files then enabling
Microsoft's ``MPEG-I Stream Splitter`` filter might be required.

Another "set" to try is enabling all the Microsoft filters.

You can click on any column label in the :guilabel:`Compatibility`
filter list to sort by that column. You can also resize a column by
dragging near the edge of its header.

You don't have to restart |EE| to see the effect of changing
filters. Once it is able to import a troublesome video, just run |CLATN|
again.

\ |EE| has a cache that it uses when importing videos. This doesn't
affect |CLATN| but if you want to force |EE| to analyze a file after
importing, you need to clear the cache by choosing
:menuselection:`&Tools --> &Options...`, clicking on the
:guilabel:`Other` tab, and clicking the :guilabel:`Clear Cache` button.

.. figure:: images/ee4clearcache.png
   :align: center
   :alt: Clearing the |EE| cache

   Clearing the |EE| cache

.. _ffdshow-codec:

The ffdshow codec
-----------------

If you are using the highly recommended `ffdshow
<http://sourceforge.net/projects/ffdshow/>`_ codecs then you might have
to configure them. In particular they have a codec pane that lists all
the supported codecs, whether they are enabled or not, and which decoder
is being used.

.. figure:: images/ffdshow-video-config-codecs.png
   :align: center
   :alt: ffdshow codecs pane

   ffdshow codecs configuration pane


It's also helpful to make sure a System Tray icon is displayed whenever
the ffdshow video codec is being used:

.. figure:: images/ffdshow-video-config-tray.png
   :align: center
   :alt: ffdshow Tray, dialogs, & paths pane

   ffdshow Tray, dialogs, & paths pane

You might want to also display a dialog whenever a "new" application
trys to use the ffdshow codec:

.. figure:: images/ffdshow-video-config-directshow.png
   :align: center
   :alt: ffdshow DirectShow control pane

   ffdshow DirectShow control pane


Common Problems
===============

Why do I get the following error messages when trying to run :command:`clatn`?
------------------------------------------------------------------------------

*'clatn' is not recognized as an internal or external command,
operable program or batch file.*

You didn't follow the instructions for copying `clatn.bat` to a
directory in your current PATH. See :ref:`this <copying-clatn>` for
more information.

*'C:\\Program Files\\CLAutoThumbnailer\\CLAutoThumbnailer.exe' is not
recognized as an internal or external command, operable program or
batch file.*

You didn't follow the instructions for editing `clatn.bat` so that it
points to the correct location of `CLAutoThumbnailer.exe`. See
:ref:`this <editing-clatn>` for more information.


.. _not-a-video:

Why do I see the "isn't  a video file" message?
-----------------------------------------------

Make sure you close any open video players that are viewing that same file.

Also see `Supported file formats
<http://expression.microsoft.com/en-us/library/cc294687>`_

Follow these steps to help troubleshoot problems with unrecognized
video files:

.. _bsp-filters-advanced:

#. Make sure |BSP|_ can play the video. Right-click on the video and
   choose :menuselection:`Options --> Filters --> Advanced` to see
   the codecs that |BSP| is using to view the video. Unfortunately,
   |BSP| typically uses its own private copy of these codecs but this
   information is still helpful when enabling filters in |EE|.

   .. figure:: images/bsp-filters-advanced.png
      :align: center
      :alt: |BSP| :menuselection:`Options --> Filters --> Advanced` 
            Dialog box

      |BSP| :menuselection:`Options --> Filters --> Advanced` Dialog box

   Remember that even though |CLATN| is only generating thumbnails,
   the proper :bi:`audio` codec also has to be installed on your
   system in order for it to open a particular video file.

   If |BSP| can't play the video file then it might be corrupt.

#. See if you can `import
   <http://expression.microsoft.com/en-us/library/cc294649>`_ the
   video using |EE|.

   If |EE| can't import the video it will sometimes display a helpful
   message explaining the reason why.

   If you are using the recommended ffdshow filter set try switching
   to the |EE| filter set and vice versa. See :ref:`here
   <ee-video-filters>` for more information.

   You can try asking about your problem on the `Microsoft Expression
   Encoder Forum
   <http://social.expression.microsoft.com/Forums/en-US/encoder/threads>`_. They
   won't be able to answer any questions about |CLATN|, but getting |EE|
   to correctly import a video will almost always also solve the problem
   for |CLATN|.

#. Use :ref:`MediaInfo <MediaInfo>` to figure out exactly what codec the
   offending video is using. Then try enabling some filters that
   appear to support that codec in |EE|. You also use the information
   displayed by |BSP| mentioned in :ref:`this step
   <bsp-filters-advanced>` to get a hint about what filters to
   enable.

#. Try :ref:`GraphStudio <GraphStudio>` to see what codecs are used
   by default to render the video. If GraphStudio can play the video,
   then you can try enabling those filters in |EE|.

#. If you are using the ffdshow codec you might have to configure
   it. See :ref:`ffdshow-codec` for more information.

#. As a last ditch effort you can try enabling all the video filters
   in |EE| and then seeing if you can import the video. If this is
   successful, then you "just" have to narrow down the list until
   only a few filters are enabled (google "binary search" for a
   "quick" way of doing this).



.. _file-type-not-supported:

Why does |EE| display the "Error: File type isn't supported." message?
----------------------------------------------------------------------

If |EE| displays an "Error: File type isn't supported." message when you
import a video, try using the Gabest splitter(s) and disable the Haali
Media Splitter(s).

This seems to happen particularly with :mono:`MPEG` encoded videos.


How do I fix broken video files?
--------------------------------

Once you've determined that a video file appears to be corrupt, all
is not lost. You can see if :ref:`DivFix++ <DivFix>` (for
:mono:`AVI`\ s), :ref:`Meteorite <meteorite>` (for :mono:`MKV`\ s),
or :ref:`asfbinwin <asfbinwin>` (for :mono:`WMV`\ s and :mono:`ASF`\
s) can fix it.


Why is |CLATN| crashing?
------------------------

If |CLATN| crashes with a lengthy "Unhandled Exception" message, then
there is probably some error in the program and you should report a bug
`here <https://github.com/rm2/CLAutoThumbnailer/issues>`__.

However, if it just crashes silently and mysteriously bounces you back
to the Command Prompt the problem is more likely with the video codec
you are using. Oftentimes, just trying again solves the problem. If not,
try switching to the |EE| filters if you are using the ffdshow codec or
vice versa.

The :ref:`ffdshow codec <ffdshow-codec>` can be sensitive to its
configuration settings. You might want to bring up the "ffdshow video
decoder configuration" Dialog box and look around and see if any of the
settings are obviously wrong. ffdshow usually has two options for
decoder for any format, so try the other if one always seems to
crash. For example, if the ``Xvid`` decoder is causing problems try
``libavcodec`` instead.


Why don't my thumbnails match what I see with my video player?
--------------------------------------------------------------

This seems to be particularly a problem with :mono:`MPEG2` encoded
(aka VOB) files. The workaround is to use the |EE| player to check
thumbnail times.


Useful tools
============

.. _GraphStudio:

`MONOGRAM GraphStudio <http://www.monogrammultimedia.com/graphstudio.html>`_
----------------------------------------------------------------------------

:menuselection:`&File --> Render Media File...` (:kbd:`Ctrl+R`)

.. figure:: images/graphstudio-hgf.png
   :align: center
   :alt: MONOGRAM GraphStudio screenshot

   MONOGRAM GraphStudio screenshot

.. _MediaInfo:

`MediaInfo <http://mediainfo.sourceforge.net/en>`__
---------------------------------------------------

.. figure:: images/mediainfo-hgf.png
   :align: center
   :alt: MediaInfo screenshot

   MediaInfo screenshot


.. _asfbinwin:

`asfbinwin <http://www.radioactivepages.com/asfbin.aspx>`__
-----------------------------------------------------------

"Intuitive, fast and reliable tool for processing ASF and WMV files.

Asfbin is a full implementation of ASF file specification completely independent from Microsoft Windows Media Format SDK. This makes Asfbin especially powerful when it comes to fixing damaged ASF/WMV files. It can repair almost all types of errors within your favourite video files."


.. _DivFix:

`DivFix++ <http://www.divfix.org/>`_
------------------------------------

"DivFix++ is FREE AVI Video Fix & Preview program."


.. _meteorite:

`Meteorite <http://meteorite.sourceforge.net/>`__
-------------------------------------------------

"Meteorite Project is DivFix++ like program but for Matroska/MKV files.
It can repair your corrupted MKV video files to make it compatible with
your player."


..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
