using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSEEncoder = Microsoft.Expression.Encoder;
using THelper = TraceHelper.TraceHelper;

namespace ThumbnailUtils
    {
    /// <summary>
    /// Thumbnail Creator class
    /// </summary>
    /// <remarks>
    /// <para>
    /// The master class used to create both Overview and 
    /// Detail thumbnail pages.
    /// </para>
    /// <para>
    /// Using a <see cref="ThumbnailSettings"/> instance that contains 
    /// all the details on what thumbnails to generate, create a 
    /// <see cref="ThumbnailCreator"/>, and then call either
    /// <see cref="GenerateOverviewThumbs"/> or
    /// <see cref="GenerateDetailThumbs"/>.
    /// </para>
    /// </remarks>
    public class ThumbnailCreator : IDisposable
        {
        #region Constants
        #endregion

        #region Static Methods
        /// <summary>
        /// Gets the full directory name from a (perhaps relative) filename.
        /// </summary>
        /// <param name="filename">The filename to get the full directory name from.</param>
        /// <returns>The full directory name.</returns>
        public static string GetDirectoryName (string filename)
            {
            string fullpath = System.IO.Path.GetFullPath (filename);
            string directory = System.IO.Path.GetDirectoryName (fullpath);
            //directory = System.IO.Path.Combine (directory, "thumbs");
            //if (!System.IO.Directory.Exists (directory))
            //    System.IO.Directory.CreateDirectory (directory);
            return directory;
            }

        /// <summary>
        /// Converts a file size to a string followed by units.
        /// </summary>
        /// <param name="filesize">The filesize.</param>
        /// <returns>File size as a <see cref="string"/>.</returns>
        public static string GetFileSizeString (long filesize)
            {
            double size;
            string units;
            string formatStr;

            if (filesize < 1024 * 1024)
                {
                size = filesize / 1024.0;
                units = "KB";
                }
            else if (filesize < 1024 * 1024 * 1024)
                {
                size = filesize / (1024.0 * 1024.0);
                units = "MB";
                }
            else
                {
                size = filesize / (1024.0 * 1024.0 * 1024.0);
                units = "GB";
                }

            if (size < 10)
                formatStr = "{0:f2}{1}";
            else if (size < 100)
                formatStr = "{0:f1}{1}";
            else
                formatStr = "{0:f0}{1}";

            return String.Format (formatStr, size, units);
            }


        private System.Drawing.Imaging.ImageCodecInfo GetEncoder (System.Drawing.Imaging.ImageFormat format)
            {
            System.Drawing.Imaging.ImageCodecInfo[] codecs = 
                System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders ();

            foreach (System.Drawing.Imaging.ImageCodecInfo codec in codecs)
                {
                if (codec.FormatID == format.Guid)
                    {
                    return codec;
                    }
                }
            return null;
            }

        /// <summary>
        /// Generates a thumbnail OBSOLETE
        /// </summary>
        /// <param name="mediaItem">The <see cref="Microsoft.Expression.Encoder.MediaItem"/>
        /// to use to generate the thumbnail.</param>
        /// <param name="time">The time of the thumbnail.</param>
        /// <param name="thumbWidth">Width of the thumbnail.</param>
        /// <param name="thumbHeight">Height of the thumbnail.</param>
        /// <param name="srcRect">The source rect to use when clipping the thumbnail.</param>
        /// <returns>A <see cref="System.Drawing.Bitmap"/>.</returns>
        /// <remarks>
        /// Generates the thumbnail at the orignal source size and then resizes it.
        /// </remarks>
        public static System.Drawing.Bitmap GenerateThumbnail (MSEEncoder.MediaItem mediaItem, 
                                                               TimeSpan time,
                                                               int thumbWidth,
                                                               int thumbHeight,
                                                               System.Drawing.Rectangle srcRect)
            {
            System.Drawing.Bitmap resized = new System.Drawing.Bitmap (thumbWidth, thumbHeight);

            using (System.Drawing.Bitmap original = 
                        mediaItem.MainMediaFile.GetThumbnail (time, mediaItem.OriginalVideoSize)) {
                using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage (resized)) {
                    // No alpha channel usage
                    graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                    //graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    //graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                    // Affects image resizing
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                    // Affects anti-aliasing of filled edges
                    //graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    using (System.Drawing.Imaging.ImageAttributes att = 
                              new System.Drawing.Imaging.ImageAttributes ()) {
                        att.SetWrapMode (System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                        graphics.DrawImage (original,
                                            new System.Drawing.Rectangle(0, 0, thumbWidth, thumbHeight),
                                            srcRect.X, srcRect.Y,
                                            srcRect.Width, srcRect.Height,
                                            System.Drawing.GraphicsUnit.Pixel,
                                            att);
                        }
                    }
                }

            return resized;
            }

        /// <summary>
        /// Generates a thumbnail 
        /// </summary>
        /// <param name="tgen">The 
        /// <see cref="Microsoft.Expression.Encoder.ThumbnailGenerator"/>
        /// to use to generate the thumbnail.</param>
        /// <param name="time">The time of the thumbnail.</param>
        /// <param name="thumbWidth">Width of the thumbnail.</param>
        /// <param name="thumbHeight">Height of the thumbnail.</param>
        /// <param name="srcRect">The source rect to use when clipping the thumbnail.</param>
        /// <returns>A <see cref="System.Drawing.Bitmap"/>.</returns>
        /// <remarks>
        /// Generates the thumbnail at the orignal source size and then resizes it.
        /// </remarks>
        public static System.Drawing.Bitmap GenerateThumbnail (MSEEncoder.ThumbnailGenerator tgen,
                                                               TimeSpan time,
                                                               int thumbWidth,
                                                               int thumbHeight,
                                                               System.Drawing.Rectangle srcRect)
            {
            System.Drawing.Bitmap resized = new System.Drawing.Bitmap (thumbWidth, thumbHeight);

            System.Drawing.Bitmap original = null;
            int counter = 0;

            while (original == null)
                {
                try
                    {
                    original = tgen.CreateThumbnail (time);
                    }
                catch (MSEEncoder.UnableToCreateThumbnailException)
                    {
                    THelper.Error ("Unable to create thumbnail at time {0}", 
                                    time.ToString (@"h\:mm\:ss\.ffff"));
                    time += new TimeSpan (0, 0, 0, 0, 50);
                    THelper.Error (" Trying time {0}",
                                    time.ToString (@"h\:mm\:ss\.ffff"));
                    counter++;
                    if (counter > 20)
                        throw;
                    }
                }

            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage (resized))
                {
                // No alpha channel usage
                graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                //graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                //graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                // Affects image resizing
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                // Affects anti-aliasing of filled edges
                //graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                using (System.Drawing.Imaging.ImageAttributes att = 
                            new System.Drawing.Imaging.ImageAttributes ())
                    {
                    att.SetWrapMode (System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                    graphics.DrawImage (original,
                                        new System.Drawing.Rectangle (0, 0, thumbWidth, thumbHeight),
                                        srcRect.X, srcRect.Y,
                                        srcRect.Width, srcRect.Height,
                                        System.Drawing.GraphicsUnit.Pixel,
                                        att);
                    }
                }
            original.Dispose();

            return resized;
            }
        #endregion Static Methods

        #region Fields
        ThumbnailSettings _tnSettings;
        System.ComponentModel.BackgroundWorker _worker;

        internal System.Drawing.Imaging.ImageCodecInfo _imageCodec;
        internal System.Drawing.Imaging.EncoderParameter _qualityParameter;
        internal System.Drawing.Imaging.EncoderParameters _qualityParameters;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ThumbnailCreator"/> class.
        /// </summary>
        /// <param name="tnSettings">The <see cref="ThumbnailSettings"/> to use.</param>
        /// <param name="worker">The <see cref="System.ComponentModel.BackgroundWorker"/>worker to use.
        /// </param>
        public ThumbnailCreator (ThumbnailSettings tnSettings, System.ComponentModel.BackgroundWorker worker)
            {
            this._tnSettings = tnSettings;
            this._worker = worker;

#if false
            _imageCodec = GetEncoder (System.Drawing.Imaging.ImageFormat.Png);
            _qualityParameter = new System.Drawing.Imaging.EncoderParameter (
                    System.Drawing.Imaging.Encoder.Quality, 75L);
            _qualityParameters = new System.Drawing.Imaging.EncoderParameters (1);
            _qualityParameters.Param[0] = _qualityParameter;
#else
            _imageCodec = GetEncoder (System.Drawing.Imaging.ImageFormat.Jpeg);
            _qualityParameter = new System.Drawing.Imaging.EncoderParameter (
                    System.Drawing.Imaging.Encoder.Quality, 75L);
            _qualityParameters = new System.Drawing.Imaging.EncoderParameters (1);
            _qualityParameters.Param[0] = _qualityParameter;
#endif

#if false
            using (System.Drawing.Bitmap bitmap1 = new System.Drawing.Bitmap (1, 1))
                {
                System.Drawing.Imaging.EncoderParameters paramList = 
                        bitmap1.GetEncoderParameterList (_imageCodec.Clsid);
                System.Drawing.Imaging.EncoderParameter[] encParams = paramList.Param;
                foreach (System.Drawing.Imaging.EncoderParameter p in encParams)
                    {
                    THelper.Information ("Type {0}, GUID {1}", p.ValueType, p.Encoder.Guid);
                    }

                paramList.Dispose ();
                }
#endif
            }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
            {
            Dispose(true);
            GC.SuppressFinalize(this);
            }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
            {
            if (disposing)
                {
                // get rid of managed resources
                }   

            // get rid of unmanaged resources
            _qualityParameter.Dispose ();
            _qualityParameters.Dispose ();
            }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="ThumbnailCreator"/> is reclaimed by garbage collection.
        /// </summary>
        ~ThumbnailCreator()
            {
            Dispose(false);
            }
        #endregion Constructors

        #region Properties
        /// <summary>
        /// Gets the <see cref="ThumbnailSettings"/>.
        /// </summary>
        /// <value>
        /// The <see cref="ThumbnailSettings"/>.
        /// </value>
        public ThumbnailSettings TNSettings
            {
            get { return _tnSettings; }
            }

        /// <summary>
        /// Gets the <see cref="System.ComponentModel.BackgroundWorker"/>.
        /// </summary>
        public System.ComponentModel.BackgroundWorker BGWorker
            {
            get { return _worker;  }
            }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Calculate <paramref name="time"/>'s percentage of total duration.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public int CalcDurationPercentage(TimeSpan time)
            {
            double seconds = (TNSettings.End - TNSettings.Start).TotalSeconds;
            double percent = (time - TNSettings.Start).TotalSeconds / seconds;
            return (int) ((percent * 100) + 0.5);
            }

        /// <summary>
        /// Creates the thumbnail grid.
        /// </summary>
        /// <param name="layoutMode">The <see cref="ThumbnailSettings.LayoutModes"/>.</param>
        /// <param name="nThumbs">The desired number of rows or columns.</param>
        /// <param name="nColumns">The number of desired columns.</param>
        /// <param name="nRows">The number of desired rows.</param>
        /// <param name="priorityThreshold">The threshold that determines when to switch
        /// between Column and Row Priority in
        /// <see cref="ThumbnailSettings.LayoutModes.Auto"/>.</param>
        /// <returns></returns>
        public ThumbnailGrid CreateThumbnailGrid (ThumbnailSettings.LayoutModes layoutMode,
                                                  int nThumbs, int nColumns, int nRows,
                                                  double priorityThreshold)
            {
            ThumbnailGrid tgrid = null;

            ThumbnailPageLayout container = 
                new ThumbnailPageLayout (TNSettings);
            double thumbAspectRatio = TNSettings.ThumbAspectRatio;

            switch (layoutMode)
                {
                case ThumbnailSettings.LayoutModes.Auto:
                    {
                    if (thumbAspectRatio < priorityThreshold)
                        {
                        THelper.Debug ("Auto Column Priority Layout (aspect ratio {0:F2} < {1}), using {2} columns",
                                        thumbAspectRatio, priorityThreshold,
                                        nThumbs);
                        tgrid =
                            ThumbnailGrid.CreateUsingNColumnsWastedHeight (container,
                                                                           TNSettings,
                                                                           nThumbs,
                                                                           thumbAspectRatio);
                        }
                    else
                        {
                        THelper.Debug ("Auto Row Priority Layout (aspect ratio {0:F2} >= {1}), using {2} rows",
                                        thumbAspectRatio, priorityThreshold,
                                        nThumbs);
                        tgrid =
                            ThumbnailGrid.CreateUsingNRowsWastedWidth (container,
                                                                       TNSettings,
                                                                       nThumbs,
                                                                       thumbAspectRatio);
                        }
                    break;
                    }

                case ThumbnailSettings.LayoutModes.Actual:
                    {
                    if (TNSettings.LayoutMode == ThumbnailSettings.LayoutModes.Actual)
                        THelper.Debug ("Actual Layout: {0}x{1}",
                                        nColumns, nRows);
                    tgrid = ThumbnailGrid.CreateUsingActual (container,
                                                             TNSettings,
                                                             nColumns,
                                                             nRows,
                                                             thumbAspectRatio);
                    break;
                    }

                case ThumbnailSettings.LayoutModes.RowPriority:
                    {
                    THelper.Debug ("Row Priority Layout, using {0} rows",
                                   nThumbs);
                    tgrid =
                        ThumbnailGrid.CreateUsingNRowsWastedWidth (container,
                                                                   TNSettings,
                                                                   nThumbs,
                                                                   thumbAspectRatio);
                    break;
                    }

                case ThumbnailSettings.LayoutModes.ColumnPriority:
                    {
                    THelper.Debug ("Column Priority Layout, using {0} columns",
                                   nThumbs);
                    tgrid =
                        ThumbnailGrid.CreateUsingNColumnsWastedHeight(container,
                                                                      TNSettings,
                                                                      nThumbs,
                                                                      thumbAspectRatio);
                    break;
                    }
                }

            container.AdjustSize (tgrid);
            return tgrid;
            }

        /// <summary>
        /// Generates the Detail thumbnail pages.
        /// </summary>
        /// <param name="avFiles">The <see cref="AVFileSet"/> to generate
        /// thumbnails for.</param>
        /// <param name="displayFilename">The display name of the <see cref="AVFileSet"/>.</param>
        /// <param name="outTemplate">The template (string format) to use when generating
        /// output filenames.</param>
        /// <param name="outputDirectory">The fullpath of the output directory.</param>
        /// <returns>
        /// The number of thumbnails created.
        /// </returns>
        public int GenerateDetailThumbs (AVFileSet avFiles,
                                        string displayFilename, 
                                        string outTemplate,
                                        string outputDirectory)
            {
            ThumbnailGrid tgrid = CreateThumbnailGrid (TNSettings.LayoutMode,
                                                       TNSettings.DetailThumbs,
                                                       TNSettings.DetailColumns,
                                                       TNSettings.DetailRows,
                                                       TNSettings.DetailThreshold);

            //ThumbnailGrid tgrid = ThumbnailGrid.CreateUsingNRows (layout, _maxDetailRows, _detailColumns,
            //                                                      aspectRatio, _mThreshold);
            //layout.adjustWidth (tgrid.NColumns, tgrid.ThumbWidth);

            System.Collections.Generic.IEnumerator<MSEEncoder.AudioVideoFile> avFilesEnumerator = 
                avFiles.GetEnumerator();
            avFilesEnumerator.MoveNext();
            MSEEncoder.AudioVideoFile avFile = avFilesEnumerator.Current;
            int avFileNum = 1;
            bool highlight = avFiles.Count > 1 ? true : false;

            System.Drawing.Size videoSize = AVFileSet.GetVideoSize(avFile);

            int nThumbsPerPage = tgrid.NColumns * tgrid.NRows;
            int nThumbs = (int) ((TNSettings.End - TNSettings.Start).TotalSeconds /
                                  TNSettings.Interval.TotalSeconds) + 1;

            // start adjustment to make thumbnails occur at _interval seconds
            int nExtraStartSeconds = (int) (TNSettings.Start.TotalSeconds %
                                            TNSettings.Interval.TotalSeconds);
            int nStartIntervals = (int) (TNSettings.Start.TotalSeconds /
                                         TNSettings.Interval.TotalSeconds);
            if (nExtraStartSeconds != 0)
                {
                nThumbs++;
                nStartIntervals++;
                }

            int nExtraEndSeconds = (int) (TNSettings.End.TotalSeconds %
                                          TNSettings.Interval.TotalSeconds);
            int nEndIntervals = (int) (TNSettings.End.TotalSeconds /
                                       TNSettings.Interval.TotalSeconds);
            TimeSpan adjustedEnd = TNSettings.End;
            if (nExtraEndSeconds != 0)
                {
                nThumbs++;
                adjustedEnd = TNSettings.End;
                //adjustedEnd = new TimeSpan (0, 0, nEndIntervals * 
                //                                 (int) tnSettings.Interval.TotalSeconds);
                }

            int nPages = (int) ((float) nThumbs / nThumbsPerPage + 0.5);
            if (nPages * nThumbsPerPage < nThumbs)
                nPages++;

            string stats;
            if (videoSize.Width != TNSettings.SrcRect.Width ||
                videoSize.Height != TNSettings.SrcRect.Height ||
                Math.Abs ((double) videoSize.Width /
                    videoSize.Height -
                    TNSettings.ThumbAspectRatio) > 0.01)
                {
                stats = String.Format ("{0}x{1} ({2:F2}:1) [{3}x{4} ({5:F2}:1)] {6}  {7}",
                    TNSettings.SrcRect.Width,
                    TNSettings.SrcRect.Height,
                    TNSettings.ThumbAspectRatio,

                    videoSize.Width,
                    videoSize.Height,
                    (double) videoSize.Width /
                        videoSize.Height,

                    //getAudioStreamStats(mediaItem),
                    GetFileSizeString (avFiles.TotalFileSize),
                    System.IO.File.GetLastWriteTime (avFile.FileName).ToString ("g")
                    );
                }
            else
                {
                stats = String.Format ("{0}x{1} ({2:F2}:1) {3}  {4}",
                    videoSize.Width,
                    videoSize.Height,
                    TNSettings.ThumbAspectRatio,
                    //getAudioStreamStats(mediaItem),
                    GetFileSizeString (avFiles.TotalFileSize),
                    System.IO.File.GetLastWriteTime (avFile.FileName).ToString ("g")
                    );
                }

            THelper.Information (
                "Generating {0} {1}x{2} thumbnails every {3} seconds on {4} {5}x{6} Detail pages.",
                nThumbs, 
                tgrid.ThumbWidth, tgrid.ThumbHeight,
                TNSettings.Interval.TotalSeconds, nPages,
                tgrid.NColumns, tgrid.NRows);
            
            ThumbnailMultiWriter mwriter = 
                new ThumbnailMultiWriter (this, tgrid,
                                          outputDirectory, displayFilename, outTemplate,
                                          avFiles.Count > 1 ? avFiles.Count : 0,
                                          TNSettings.Interval,
                                          stats, avFiles.TotalDuration, nPages);

            TimeSpan currentTime = TNSettings.Start;
            int thumbCount = 0;
            TimeSpan fileStartTime = new TimeSpan(0,0,0);
            MSEEncoder.ThumbnailGenerator thumbGenerator = avFile.CreateThumbnailGenerator (videoSize);

            while (currentTime > fileStartTime + AVFileSet.GetVideoDuration(avFile))
                {
                if (thumbGenerator != null)
                    {
                    thumbGenerator.Dispose();
                    thumbGenerator = null;
                    }

                if (!avFilesEnumerator.MoveNext ())
                    {
                    avFile = null;
                    break;
                    }

                fileStartTime += AVFileSet.GetVideoDuration(avFile);
                avFile = avFilesEnumerator.Current;
                avFileNum++;
                highlight = true;
                thumbGenerator = avFile.CreateThumbnailGenerator (videoSize);
                }

            if (nExtraStartSeconds != 0 && thumbGenerator != null)
                {
                using (System.Drawing.Bitmap resized = 
                    GenerateThumbnail (thumbGenerator, currentTime - fileStartTime,
                                       tgrid.ThumbWidth, tgrid.ThumbHeight, TNSettings.SrcRect))
                    {
                    if (avFiles.Count > 1)
                        mwriter.Add (resized, currentTime, highlight, avFileNum, fileStartTime);
                    else
                        mwriter.Add (resized, currentTime, highlight, 0, TimeSpan.Zero);
                    thumbCount++;
                    highlight = false;
                    }
                currentTime = new TimeSpan (0, 0, 0, 0,
                                            (int) (nStartIntervals *
                                                   TNSettings.Interval.TotalMilliseconds));
                }

            while (currentTime <= adjustedEnd && thumbGenerator != null)
                {
                while (currentTime > fileStartTime + AVFileSet.GetVideoDuration(avFile))
                    {
                    if (thumbGenerator != null)
                        {
                        thumbGenerator.Dispose();
                        thumbGenerator = null;
                        }

                    if (!avFilesEnumerator.MoveNext ())
                        {
                        avFile = null;
                        break;
                        }

                    fileStartTime += AVFileSet.GetVideoDuration(avFile);
                    avFile = avFilesEnumerator.Current;
                    avFileNum++;
                    highlight = true;
                    thumbGenerator = avFile.CreateThumbnailGenerator (videoSize);
                    }
                if (thumbGenerator == null)
                    break;

                if (BGWorker != null)
                    {
                    if (BGWorker.CancellationPending)
                        {
                        if (thumbGenerator != null)
                            {
                            thumbGenerator.Dispose ();
                            thumbGenerator = null;
                            }
                        mwriter.Close ();
                        return thumbCount;
                        }
                    }

                using (System.Drawing.Bitmap resized = 
                    GenerateThumbnail (thumbGenerator, currentTime - fileStartTime,
                                       tgrid.ThumbWidth, tgrid.ThumbHeight, TNSettings.SrcRect))
                    {
                    if (avFiles.Count > 1)
                        mwriter.Add (resized, currentTime, highlight, avFileNum, fileStartTime);
                    else
                        mwriter.Add (resized, currentTime, highlight, 0, TimeSpan.Zero);
                    thumbCount++;
                    highlight = false;
                    }
                currentTime += TNSettings.Interval;
                }

            if (nExtraEndSeconds != 0 && thumbCount < nThumbs && 
                thumbGenerator != null)
                {
                using (System.Drawing.Bitmap resized = 
                    GenerateThumbnail (thumbGenerator, TNSettings.End - fileStartTime,
                                       tgrid.ThumbWidth, tgrid.ThumbHeight, TNSettings.SrcRect))
                    {
                    if (avFiles.Count > 1)
                        mwriter.Add (resized, TNSettings.End, highlight, avFileNum, fileStartTime);
                    else
                        mwriter.Add (resized, TNSettings.End, highlight, 0, TimeSpan.Zero);
                    thumbCount++;
                    highlight = false;
                    }
                }
            if (thumbGenerator != null)
                {
                thumbGenerator.Dispose ();
                thumbGenerator = null;
                }

            mwriter.Close ();
            return thumbCount;
            }

        /// <summary>
        /// Generates the overview thumbnail page.
        /// </summary>
        /// <param name="avFiles">The <see cref="AVFileSet"/> to generate 
        /// thumbnails for.</param>
        /// <param name="displayFilename">The display name of the <see cref="AVFileSet"/>.</param>
        /// <param name="outTemplate">The template (string format) to use when generating
        /// output filenames.</param>
        /// <param name="outputDirectory">The fullpath of the output directory.</param>
        /// <returns>The number of thumbnails created.</returns>
        public int GenerateOverviewThumbs (AVFileSet avFiles,
                                           string displayFilename, 
                                           string outTemplate,
                                           string outputDirectory)
            {
            ThumbnailGrid tgrid = CreateThumbnailGrid (TNSettings.LayoutMode,
                                                       TNSettings.OverviewThumbs,
                                                       TNSettings.OverviewColumns,
                                                       TNSettings.OverviewRows,
                                                       TNSettings.OverviewThreshold);

            System.Collections.Generic.IEnumerator<MSEEncoder.AudioVideoFile> avFilesEnumerator = 
                avFiles.GetEnumerator();
            avFilesEnumerator.MoveNext();
            MSEEncoder.AudioVideoFile avFile = avFilesEnumerator.Current;
            int avFileNum = 1;
            bool highlight = avFiles.Count > 1 ? true : false;

            System.Drawing.Size videoSize = AVFileSet.GetVideoSize (avFile);

#if true
            string stats;
            if (videoSize.Width != TNSettings.SrcRect.Width ||
                videoSize.Height != TNSettings.SrcRect.Height ||
                Math.Abs ((double) videoSize.Width / videoSize.Height -
                          TNSettings.ThumbAspectRatio) > 0.01)
                {
                stats = String.Format ("{0}x{1} ({2:F2}:1) [{3}x{4} ({5:F2}:1)] {6}  {7}",
                    TNSettings.SrcRect.Width,
                    TNSettings.SrcRect.Height,
                    TNSettings.ThumbAspectRatio,

                    videoSize.Width,
                    videoSize.Height,
                    (double) videoSize.Width /
                        videoSize.Height,

                    //getAudioStreamStats(mediaItem),
                    GetFileSizeString (avFiles.TotalFileSize),
                    System.IO.File.GetLastWriteTime (avFile.FileName).ToString ("g")
                    );
                }
            else
                {
                stats = String.Format ("{0}x{1} ({2:F2}:1) {3}  {4}",
                    videoSize.Width,
                    videoSize.Height,
                    TNSettings.ThumbAspectRatio,
                    //getAudioStreamStats(mediaItem),
                    GetFileSizeString (avFiles.TotalFileSize),
                    System.IO.File.GetLastWriteTime (avFile.FileName).ToString ("g")
                    );
                }

#else
            string stats = String.Format ("{0}x{1} ({2:F2}:1) [{3}x{4} {5}x{6}]  {7}  {8}",
                videoSize.Width,
                videoSize.Height,
                (double) videoSize.Width /
                    videoSize.Height,
                //getAudioStreamStats(mediaItem),
                tgrid.ThumbWidth, tgrid.ThumbHeight,
                tgrid.NColumns, tgrid.NRows,
                GetFileSizeString (mediaItem.MainMediaFile.FileSize),
                System.IO.File.GetLastWriteTime (displayFilename).ToString ("g")
                );
#endif

            THelper.Information ("Generating {0} {1}x{2} thumbnails on a {3}x{4} Overview page.",
                                 tgrid.NThumbs,
                                 tgrid.ThumbWidth, tgrid.ThumbHeight,
                                 tgrid.NColumns, tgrid.NRows);
                                 
            ThumbnailWriter writer = 
                new ThumbnailWriter (this, tgrid, outputDirectory, displayFilename, outTemplate,
                                     avFiles.Count > 1 ? avFiles.Count : 0,
                                     stats, avFiles.TotalDuration);

            double intervalSeconds = 
                ((TNSettings.End.TotalSeconds - TNSettings.Start.TotalSeconds) /
                 (tgrid.NColumns * tgrid.NRows - 1));
            int milliseconds = 
                (int) ((intervalSeconds - Math.Truncate (intervalSeconds)) * 1000 + 0.5);
            TimeSpan interval = new TimeSpan (0, 0, 0, (int) intervalSeconds, milliseconds);

            int nThumbsAdded = 0;
            TimeSpan currentTime = TNSettings.Start;
            TimeSpan fileStartTime = new TimeSpan(0,0,0);
            MSEEncoder.ThumbnailGenerator thumbGenerator = avFile.CreateThumbnailGenerator (videoSize);

            while (currentTime <= TNSettings.End)
                {
                while (currentTime > fileStartTime + AVFileSet.GetVideoDuration(avFile))
                    {
                    if (thumbGenerator != null)
                        {
                        thumbGenerator.Dispose();
                        thumbGenerator = null;
                        }

                    if (!avFilesEnumerator.MoveNext ())
                        {
                        avFile = null;
                        break;
                        }

                    fileStartTime += AVFileSet.GetVideoDuration(avFile);
                    avFile = avFilesEnumerator.Current;
                    avFileNum++;
                    highlight = true;
                    thumbGenerator = avFile.CreateThumbnailGenerator (videoSize);
                    }
                if (thumbGenerator == null)
                    break;
                if (BGWorker != null)
                    {
                    if (BGWorker.CancellationPending)
                        {
                        if (thumbGenerator != null)
                            {
                            thumbGenerator.Dispose ();
                            thumbGenerator = null;
                            }
                        writer.Close ();
                        return nThumbsAdded;
                        }
                    }

                using (System.Drawing.Bitmap resized = 
                    GenerateThumbnail (thumbGenerator, currentTime - fileStartTime,
                                       tgrid.ThumbWidth, tgrid.ThumbHeight, TNSettings.SrcRect))
                    {
                    if (avFiles.Count > 1)
                        writer.Add (resized, currentTime, highlight, avFileNum);
                    else
                        writer.Add (resized, currentTime, highlight, 0);
                    highlight = false;
                    nThumbsAdded++;
                    }
                currentTime += interval;
                }

            // Last thumb should always be end time.
            if (nThumbsAdded < tgrid.NThumbs &&
                thumbGenerator != null)
                {
                using (System.Drawing.Bitmap resized = 
                       GenerateThumbnail (thumbGenerator, TNSettings.End - fileStartTime,
                                          tgrid.ThumbWidth, tgrid.ThumbHeight, TNSettings.SrcRect))
                    {
                    if (avFiles.Count > 1)
                        writer.Add (resized, TNSettings.End, highlight, avFileNum);
                    else
                        writer.Add (resized, TNSettings.End, highlight, 0);
                    nThumbsAdded++;
                    }
                }
            if (thumbGenerator != null)
                {
                thumbGenerator.Dispose ();
                thumbGenerator = null;
                }

            writer.Close ();

            return tgrid.NThumbs;
            }

#if false
        /// <summary>
        /// Generates the multi-page thumbnails OBSOLETE.
        /// </summary>
        /// <param name="mediaItem">The 
        /// <see cref="Microsoft.Expression.Encoder.MediaItem"/> to generate 
        /// thumbnails for.</param>
        /// <param name="filename">The display name of the file to generate thumbs for.</param>
        /// <param name="outputDirectory">The output directory.</param>
        /// <returns>The number of thumbnails created.</returns>
        public int GenerateMultiThumbs (MSEEncoder.MediaItem mediaItem,
                                        string filename, string outputDirectory)
            {
            double thumbAspectRatio = TNSettings.ThumbAspectRatio;

            ThumbnailPageLayout container = new ThumbnailPageLayout (TNSettings);
            ThumbnailGrid tgrid;

            if (thumbAspectRatio < 1.44)
                tgrid =
                    ThumbnailGrid.CreateUsingNColumns (container, TNSettings,
                                                       thumbAspectRatio, true,
                                                       _debug);
            else
                tgrid =
                    ThumbnailGrid.CreateUsingNRows (container, TNSettings,
                                                    thumbAspectRatio, true,
                                                    _debug);

            container.AdjustSize (tgrid);

            //ThumbnailGrid tgrid = ThumbnailGrid.CreateUsingNRows (container, _maxMultiRows, _multiColumns,
            //                                                      aspectRatio, _mThreshold);
            //container.adjustWidth (tgrid.NColumns, tgrid.ThumbWidth);

            if (outputDirectory == null)
                outputDirectory = GetDirectoryName (filename);
            string fixedFilename = System.IO.Path.GetFileNameWithoutExtension (filename);
            fixedFilename = fixedFilename.Replace ("{", "(");
            fixedFilename = fixedFilename.Replace ("}", ")");
            string outTemplate = fixedFilename + "_page{0:D4}.jpg";

            int nThumbsPerPage = tgrid.NColumns * tgrid.NRows;
            int nThumbs = (int) ((TNSettings.End - TNSettings.Start).TotalSeconds /
                                  TNSettings.Interval.TotalSeconds) + 1;

            // start adjustment to make thumbnails occur at _interval seconds
            int nExtraStartSeconds = (int) (TNSettings.Start.TotalSeconds %
                                            TNSettings.Interval.TotalSeconds);
            int nStartIntervals = (int) (TNSettings.Start.TotalSeconds /
                                         TNSettings.Interval.TotalSeconds);
            if (nExtraStartSeconds != 0)
                {
                nThumbs++;
                nStartIntervals++;
                }

            int nExtraEndSeconds = (int) (TNSettings.End.TotalSeconds %
                                          TNSettings.Interval.TotalSeconds);
            int nEndIntervals = (int) (TNSettings.End.TotalSeconds /
                                       TNSettings.Interval.TotalSeconds);
            TimeSpan adjustedEnd = TNSettings.End;
            if (nExtraEndSeconds != 0)
                {
                nThumbs++;
                adjustedEnd = TNSettings.End;
                //adjustedEnd = new TimeSpan (0, 0, nEndIntervals * 
                //                                 (int) tnSettings.Interval.TotalSeconds);
                }

            int nPages = (int) ((float) nThumbs / nThumbsPerPage + 0.5);
            if (nPages * nThumbsPerPage < nThumbs)
                nPages++;

            string stats;
            if (mediaItem.OriginalVideoSize.Width != TNSettings.SrcRect.Width ||
                mediaItem.OriginalVideoSize.Height != TNSettings.SrcRect.Height ||
                Math.Abs ((double) mediaItem.OriginalVideoSize.Width /
                    mediaItem.OriginalVideoSize.Height -
                    TNSettings.ThumbAspectRatio) > 0.01)
                {
                stats = String.Format ("{0}x{1} ({2:F2}:1) [{3}x{4} ({5:F2}:1)] {6}  {7}",
                    TNSettings.SrcRect.Width,
                    TNSettings.SrcRect.Height,
                    TNSettings.ThumbAspectRatio,

                    mediaItem.OriginalVideoSize.Width,
                    mediaItem.OriginalVideoSize.Height,
                    (double) mediaItem.OriginalVideoSize.Width /
                        mediaItem.OriginalVideoSize.Height,

                        //getAudioStreamStats(mediaItem),
                    GetFileSizeString (mediaItem.MainMediaFile.FileSize),
                    System.IO.File.GetLastWriteTime (filename).ToString ("g")
                    );
                }
            else
                {
                stats = String.Format ("{0}x{1} ({2:F2}:1) {3}  {4}",
                    mediaItem.OriginalVideoSize.Width,
                    mediaItem.OriginalVideoSize.Height,
                    TNSettings.ThumbAspectRatio,
                    //getAudioStreamStats(mediaItem),
                    GetFileSizeString (mediaItem.MainMediaFile.FileSize),
                    System.IO.File.GetLastWriteTime (filename).ToString ("g")
                    );
                }

            THelper.Information ("Duration {0} - Generating {1} thumbs every {2} seconds on {3} pages.",
                                mediaItem.FileDuration.ToString (@"h\:mm\:ss"),
                                nThumbs, TNSettings.Interval.TotalSeconds, nPages);

            ThumbnailMultiWriter mwriter = 
                new ThumbnailMultiWriter (this, tgrid,
                                          outputDirectory, filename, outTemplate,
                                          TNSettings.Interval,
                                          stats, mediaItem.FileDuration, nPages,
                                          TNSettings.ScaleFactor, _debug);

            TimeSpan currentTime = TNSettings.Start;
            int thumbCount = 0;

            if (nExtraStartSeconds != 0)
                {
                using (System.Drawing.Bitmap resized = 
                    GenerateThumbnail (mediaItem, currentTime,
                                       tgrid.ThumbWidth, tgrid.ThumbHeight, TNSettings.SrcRect))
                    {
                    mwriter.Add (resized, currentTime, 0, TimeSpan.Zero);
                    thumbCount++;
                    }
                currentTime = new TimeSpan (0, 0, 0, 0,
                                            (int) (nStartIntervals *
                                                   TNSettings.Interval.TotalMilliseconds));
                }

            while (currentTime <= adjustedEnd)
                {
                using (System.Drawing.Bitmap resized = 
                    GenerateThumbnail (mediaItem, currentTime,
                                       tgrid.ThumbWidth, tgrid.ThumbHeight, TNSettings.SrcRect))
                    {
                    mwriter.Add (resized, currentTime, 0, TimeSpan.Zero);
                    thumbCount++;
                    }
                currentTime += TNSettings.Interval;
                }

            if (nExtraEndSeconds != 0 && thumbCount < nThumbs)
                {
                using (System.Drawing.Bitmap resized = 
                    GenerateThumbnail (mediaItem, TNSettings.End,
                                       tgrid.ThumbWidth, tgrid.ThumbHeight, TNSettings.SrcRect))
                    {
                    mwriter.Add (resized, TNSettings.End, 0, TimeSpan.Zero);
                    thumbCount++;
                    }
                }

            mwriter.Close ();
            return thumbCount;
            }

        /// <summary>
        /// Generates the overview thumbnail page OBSOLETE.
        /// </summary>
        /// <param name="mediaItem">The 
        /// <see cref="Microsoft.Expression.Encoder.MediaItem"/> to generate 
        /// thumbnails for.</param>
        /// <param name="filename">The display name of the file to generate thumbs for.</param>
        /// <param name="outputDirectory">The output directory.</param>
        /// <returns>The number of thumbnails created.</returns>
        public int GenerateOverviewThumbs (MSEEncoder.MediaItem mediaItem,
                                           string filename, string outputDirectory)
            {
            double thumbAspectRatio = TNSettings.ThumbAspectRatio;

            ThumbnailPageLayout container = 
                new ThumbnailPageLayout (TNSettings);

            ThumbnailGrid tgrid;
            if (thumbAspectRatio < 1.36)
                tgrid =
                    ThumbnailGrid.CreateUsingNColumns (container, TNSettings,
                                                       thumbAspectRatio, false,
                                                       _debug);
            else
                tgrid =
                    ThumbnailGrid.CreateUsingNRows (container, TNSettings,
                                                    thumbAspectRatio, false,
                                                    _debug);
            container.AdjustSize (tgrid);


            if (outputDirectory == null)
                outputDirectory = GetDirectoryName (filename);
            string outTemplate =  System.IO.Path.GetFileNameWithoutExtension (filename) +
                                  "_overview.jpg";

#if true
            string stats;
            if (mediaItem.OriginalVideoSize.Width != TNSettings.SrcRect.Width ||
                mediaItem.OriginalVideoSize.Height != TNSettings.SrcRect.Height ||
                Math.Abs ((double) mediaItem.OriginalVideoSize.Width /
                    mediaItem.OriginalVideoSize.Height -
                    TNSettings.ThumbAspectRatio) > 0.01)
                {
                stats = String.Format ("{0}x{1} ({2:F2}:1) [{3}x{4} ({5:F2}:1)] {6}  {7}",
                    TNSettings.SrcRect.Width,
                    TNSettings.SrcRect.Height,
                    TNSettings.ThumbAspectRatio,

                    mediaItem.OriginalVideoSize.Width,
                    mediaItem.OriginalVideoSize.Height,
                    (double) mediaItem.OriginalVideoSize.Width /
                        mediaItem.OriginalVideoSize.Height,

                        //getAudioStreamStats(mediaItem),
                    GetFileSizeString (mediaItem.MainMediaFile.FileSize),
                    System.IO.File.GetLastWriteTime (filename).ToString ("g")
                    );
                }
            else
                {
                stats = String.Format ("{0}x{1} ({2:F2}:1) {3}  {4}",
                    mediaItem.OriginalVideoSize.Width,
                    mediaItem.OriginalVideoSize.Height,
                    TNSettings.ThumbAspectRatio,
                    //getAudioStreamStats(mediaItem),
                    GetFileSizeString (mediaItem.MainMediaFile.FileSize),
                    System.IO.File.GetLastWriteTime (filename).ToString ("g")
                    );
                }

#else
            string stats = String.Format ("{0}x{1} ({2:F2}:1) [{3}x{4} {5}x{6}]  {7}  {8}",
                mediaItem.OriginalVideoSize.Width,
                mediaItem.OriginalVideoSize.Height,
                (double) mediaItem.OriginalVideoSize.Width /
                    mediaItem.OriginalVideoSize.Height,
                //getAudioStreamStats(mediaItem),
                tgrid.ThumbWidth, tgrid.ThumbHeight,
                tgrid.NColumns, tgrid.NRows,
                GetFileSizeString (mediaItem.MainMediaFile.FileSize),
                System.IO.File.GetLastWriteTime (filename).ToString ("g")
                );
#endif

            THelper.Information ("Generating {0} overview page thumbs.",
                                 tgrid.NThumbs);

            ThumbnailWriter writer = 
                new ThumbnailWriter (this, tgrid, outputDirectory, filename, outTemplate,
                                     stats, mediaItem.FileDuration,
                                     TNSettings.ScaleFactor, _debug);

            double intervalSeconds = 
                ((TNSettings.End.TotalSeconds - TNSettings.Start.TotalSeconds) /
                 (tgrid.NColumns * tgrid.NRows - 1));
            int milliseconds = 
                (int) ((intervalSeconds - Math.Truncate (intervalSeconds)) * 1000 + 0.5);
            TimeSpan interval = new TimeSpan (0, 0, 0, (int) intervalSeconds, milliseconds);

            int nThumbsAdded = 0;
            TimeSpan currentTime = TNSettings.Start;

            while (currentTime <= TNSettings.End)
                {
                using (System.Drawing.Bitmap resized = 
                    GenerateThumbnail (mediaItem, currentTime,
                                       tgrid.ThumbWidth, tgrid.ThumbHeight, TNSettings.SrcRect))
                    {
                    writer.Add (resized, currentTime);
                    nThumbsAdded++;
                    }
                currentTime += interval;
                }

            // Last thumb should always be end time.
            if (nThumbsAdded < tgrid.NThumbs)
                {
                using (System.Drawing.Bitmap resized = 
                    GenerateThumbnail (mediaItem, TNSettings.End,
                                       tgrid.ThumbWidth, tgrid.ThumbHeight, TNSettings.SrcRect))
                    {
                    writer.Add (resized, TNSettings.End);
                    nThumbsAdded++;
                    }
                }

            writer.Close ();

            return tgrid.NThumbs;
            }
#endif
        #endregion Methods
        }
    }
