.. default-role:: filename

==============
 Introduction
==============

|CLATN| is a command-line program that makes it easy to generate nicely
sized thumbnails of a video file (or even entire directories of video
files). Instead of having to experiment with the number of columns and
rows until you get thumbnails that are not too small or too large,
|CLATN| will do its best to figure this out for you automatically.

You specify the number of rows (or columns) of thumbnails you want along
with the dimensions of the screen you'll be using to view the thumbnail
pages --- or just use the defaults --- and |CLATN| will do the rest. It
automatically chooses the best number of columns (or rows) that will
fit.

It can create the typical :doc:`ref-overview` that has thumbnails of an
entire video on a single page. In addition, it can also create as many
:doc:`ref-detail` as required to have thumbnails made every set interval
(for example, every 60 seconds).

.. table::
   :class: centered, centercells, noborders

   +------------------------------+----------------------------+
   |          |over1|             |       |detail1|            |
   +------------------------------+----------------------------+
   |          |over2|             |       |detail2|            |
   +------------------------------+----------------------------+
   | **Overview thumbnail pages** | **Detail thumbnail pages** |
   +------------------------------+----------------------------+

.. |over1|   image:: images/his_girl_friday_512kb_overview_9x9.jpg
             :scale: 20%

.. |detail1| image:: images/his_girl_friday_512kb_page0001_00_00_05.jpg
             :scale: 20%

.. |over2|   image:: images/McLintock_512kb_overview.jpg
             :scale: 20%

.. |detail2| image:: images/McLintock_page0002_00_02_30.jpg
             :scale: 20%


Only in this way is it possible to process entire directories of video
files and still get nicely sized thumbs for all of them. This is
especially true if your collection contains videos of different
:term:`aspect ratios <aspect ratio>` such as full-screen 1.33 (4:3),
high-definition 1.77 (16:9), wide-screen DVD 2.35, or something
in-between. Without automatic row/column determination, most of those
types of files will come out with badly sized thumbnail pages.


Features
========

+ Allows fully :doc:`"automatic" operation <ref-layout>` based on the
  video :term:`aspect ratio` but is also customizable.

+ Can generate an :doc:`Overview <ref-overview>` thumbnail page of an
  entire video.

+ Can generate :doc:`Detail <ref-detail>` thumbnail pages where the
  thumbnails are created using a specified time interval.

+ Can process :doc:`entire directories <how-thumbnaildir>` (and
  sub-directories).

+ Can process :doc:`multi-part video files <how-multipart>` (like
  :doc:`DVD .vob sets <how-dvd>`) as a single long video.

+ Allows setting :doc:`starting and ending times <how-skipcredits>` for
  thumbnails.

+ Allows cropping to :doc:`remove black bars <how-blackbars>` and
  stretching to :doc:`fix distortion <how-distortion>`.

+ Supports :doc:`Command Files <how-skipcreditscf>` for batch processing
  of files requiring custom settings.

+ Allows creation of thumbnails at frequent intervals (sub-second) while
  still preserving thumbnail time accuracy (though some formats are
  better than others at this).

+ Is fully documented.

+ Has fully commented C# source code with documentation project files
  for `Doxygen <http://www.doxygen.org/>`_ and `Sandcastle Help File
  Builder <http://shfb.codeplex.com/>`_. However, now that I'm done with
  the initial release I probably need to refactor various classes.


Prerequisites
=============

+ Knowledge of how to enter commands in a Windows Command Prompt window.

+ The free Microsoft Expression Encoder 4 with Service Pack 1 (SP1)
  which can be downloaded from `here
  <http://www.microsoft.com/expression/service-packs/Encoder.aspx>`_ or
  `Microsoft Expression Encoder 4 Pro`_

+ The `Microsoft .NET Framework 4.0 runtime
  <http://www.microsoft.com/downloads/en/details.aspx?FamilyID=9CFB2D51-5FF4-4491-B0E5-B386F32C0992>`_.

+ :term:`Video codecs <codec>` for the video types you want to generate
  thumbnails for. If you can play your videos using Windows Media Player
  you probably already have all the codecs you need. Otherwise, I
  recommend the |KLITE|_.

+ While |CLATN| should run on Windows Vista or Windows 7, it has only
  been tested on Windows XP Pro SP3 32-bit.


Downloads
=========

Binary Releases
---------------

Binary executables for |CLATN| are available from `GitHub
<http://www.github.com>`_.  You download either a `standard installer
<https://github.com/downloads/rm2/CLAutoThumbnailer/CLAutoThumbnailerSetup-v1.0-20110610.msi>`_
(`CLAutoThumbnailerSetup.msi`) or a `zip archive
<https://github.com/downloads/rm2/CLAutoThumbnailer/CLAutoThumbnailerSetup-v1.0-20110610.zip>`_
(`CLAutoThumbnailer.zip`).


Source Code
-----------

The C# source code and Visual Studio 2010 project files for |CLATN| are
available on `GitHub <http://www.github.com>`_ either for `browsing
<https://github.com/rm2/CLAutoThumbnailer>`__ or `download
<https://github.com/rm2/CLAutoThumbnailer/zipball/master>`__.


Sphinx reStructuredText Documentation
-------------------------------------

The Sphinx reStructuredText documentation files for |CLATN| are
available on `GitHub <http://www.github.com>`_ either for `browsing
<https://github.com/rm2/CLAutoThumbnailerDoc>`__ or `download
<https://github.com/rm2/CLAutoThumbnailerDoc/zipball/master>`__.


.. _installation-instructions:

Installation
============

#. Download and install the free `Microsoft Expression Encoder 4 with
   Service Pack 1 (SP1)
   <http://www.microsoft.com/expression/service-packs/Encoder.aspx>`_ or
   `Microsoft Expression Encoder 4 Pro`_.

#. By default |EE| enables all video filters. This may work fine
   initially but eventually you should see :ref:`ee-video-filters` and
   learn how to fine tune this. 99% of any problems you have with
   |CLATN| will be the result of this (and the rest are from broken
   video files).

#. If you downloaded `CLAutoThumbnailerSetup.msi` then just run it (if
   you've previously installed |CLATN| you first have to uninstall
   it). The setup program will automatically download the Microsoft .NET
   Framework 4.0 runtime if it determines it isn't installed on your
   system.

   Otherwise, if you got `CLAutoThumbnailer.zip`, unpack it to a folder
   of your choice. Also be sure to manually install the `Microsoft .NET
   Framework 4.0 runtime
   <http://www.microsoft.com/downloads/en/details.aspx?FamilyID=9CFB2D51-5FF4-4491-B0E5-B386F32C0992>`_
   if you haven't already.

.. _editing-clatn:

4. If necessary edit `<InstallDir>\\clautn.bat` to point at the correct
   location of `CLAutoThumbnailer.exe`. It assumes that's in
   `C:\\Program Files\\CLAutoThumbnailer`.

.. _copying-clatn:

5. Copy `<InstallDir>\\clautn.bat` to some folder that is in the command
   path specified by the `PATH
   <http://msdn.microsoft.com/en-us/library/bb490963.aspx>`_
   `environment variable <http://support.microsoft.com/kb/310519>`_.

To then run |CLATN|, all you have to do is open up a Command Prompt
window, switch to a folder that has a video file in it, and type::

   clatn videofile.ext

See the following :doc:`quickstart` section for more details.


Uninstallation
==============

Just use the Control Panel's "Add or Remove Programs" dialog box and
remove ``CLAutoThumbnailer``.


Feedback
========

   Report bugs or suggestions for improvements at
   https://github.com/rm2/CLAutoThumbnailer/issues\ .

   Messages can also be sent to me via `GitHub.com
   <https://github.com/inbox/new/rm2>`_.

   If you are having problems with :ref:`codecs <dealing-with-codecs>`,
   particularly if |EE| is :ref:`unable to import a video
   <not-a-video>`, then you can try asking about your problem on the
   `Microsoft Expression Encoder Forum
   <http://social.expression.microsoft.com/Forums/en-US/encoder/threads>`_.
   They won't be able to answer any questions about |CLATN|, but getting
   |EE| to correctly import a video will almost always also solve the
   problem for |CLATN|.


..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
