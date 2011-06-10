using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THelper = TraceHelper.TraceHelper;

namespace ThumbnailUtils
    {
    /// <summary>
    /// Multi-Page Thumbnail Writer
    /// </summary>
    internal class ThumbnailMultiWriter
        {
        #region Fields
        ThumbnailCreator _creator;
        ThumbnailGrid _tgrid;
        string _directory;
        string _displayFilename;
        string _outTemplate;
        int _nFiles;
        TimeSpan _interval;
        string _stats;
        TimeSpan _duration;
        int _nPages;

        int _pageNum;
        ThumbnailPage _thumbnailPage;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ThumbnailMultiWriter"/> class.
        /// </summary>
        /// <param name="creator">The <see cref="ThumbnailCreator"/>
        /// (only used to get a jpeg compression encoder).</param>
        /// <param name="tgrid">The <see cref="ThumbnailGrid"/>.</param>
        /// <param name="directory">The directory to write thumbnail pages.</param>
        /// <param name="displayFilename">The display name of the <see cref="AVFileSet"/>
        /// from which the thumbnails are generated.</param>
        /// <param name="outTemplate">The template used to generate page filenames.</param>
        /// <param name="nFiles">The number of files in set (>0 for multi-part videos).</param>
        /// <param name="interval">The interval between thumbnails.</param>
        /// <param name="stats">The stats of the <see cref="AVFileSet"/> to display
        /// in header.</param>
        /// <param name="duration">The duration of the <see cref="AVFileSet"/>.</param>
        /// <param name="nPages">The total number of thumbnail pages.</param>
        public ThumbnailMultiWriter (ThumbnailCreator creator,
                                     ThumbnailGrid tgrid,
                                     string directory,
                                     string displayFilename,
                                     string outTemplate,
                                     int nFiles,
                                     TimeSpan interval,
                                     string stats, TimeSpan duration, int nPages)
            {
            this._creator = creator;
            this._tgrid = tgrid;
            this._directory = directory;
            this._displayFilename = displayFilename;
            this._outTemplate = outTemplate;
            this._nFiles = nFiles;
            this._interval = interval;
            this._stats = stats;
            this._duration = duration;
            this._nPages = nPages;

            this._pageNum = 1;
            _thumbnailPage = null;
            }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Add a <see cref="System.Drawing.Bitmap"/> thumbnail to the current page.
        /// </summary>
        /// <param name="thumbnail">The <see cref="System.Drawing.Bitmap"/> 
        /// thumbnail.</param>
        /// <param name="time">The <see cref="TimeSpan">time</see> the thumbnail was
        /// captured.</param>
        /// <param name="highlight">if set to <c>true</c> highlight thumbnail border.</param>
        /// <param name="fileNum">The file number
        /// (>0 for multi-file <see cref="AVFileSet"/>s).</param>
        /// <param name="fileStartTime">The file start time.</param>
        public void Add (System.Drawing.Bitmap thumbnail, TimeSpan time, bool highlight,
                         int fileNum, TimeSpan fileStartTime)
            {
            if (_thumbnailPage == null)
                {
                _thumbnailPage = CreateThumbnailPage (time);
                }

            int percentage = _creator.CalcDurationPercentage (time);
            if (_creator.BGWorker == null)
                {
                Console.Write("\b\b\b\b\b\b");
                Console.Write (String.Format("{0} {1,3}%", THelper.GetNextProgressStr (), percentage));
                }
            else
                {
                _creator.BGWorker.ReportProgress (percentage);
                }

            if (_thumbnailPage.Add (thumbnail, time, fileNum, fileStartTime, highlight))
                {
                _thumbnailPage.Close ();
                _pageNum++;
                time += _interval;
                _thumbnailPage = CreateThumbnailPage (time);
                }
            }

        /// <summary>
        /// Creates a multi-page thumbnail page.
        /// </summary>
        /// <param name="time">The <see cref="TimeSpan">time</see> of the first thumbnail 
        /// on page.</param>
        /// <returns>new <see cref="ThumbnailPage"/>.</returns>
        private ThumbnailPage CreateThumbnailPage (TimeSpan time)
            {
            string filename = String.Format (_outTemplate, _pageNum);
            if (_creator.TNSettings.DetailFileTimestamps)
                {
                string ext = System.IO.Path.GetExtension (filename);
                filename = System.IO.Path.GetFileNameWithoutExtension (filename);
                filename += String.Format (@"{0:\_hh\_mm\_ss}{1}", time, ext);
                }
            filename = System.IO.Path.Combine (_directory, filename);

            ThumbnailPage page = new ThumbnailPage (_creator, _tgrid,
                                                    _displayFilename, filename, _nFiles,
                                                    time, 
                                                    _pageNum, 
                                                    _duration, _nPages,
                                                    _stats);
            return page;
            }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
            {
            if (_thumbnailPage != null)
                {
                _thumbnailPage.Close ();
                _thumbnailPage = null;
                }

            if (_creator.BGWorker == null)
                {
                Console.Write ("\b\b\b\b\b\b      \b\b\b\b\b\b");
                //Console.WriteLine ("");
                }
            }
        #endregion Methods
        }
    }
