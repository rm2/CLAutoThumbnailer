using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THelper = TraceHelper.TraceHelper;

namespace ThumbnailUtils
    {
    internal class ThumbnailWriter
        {
        #region Fields
        ThumbnailCreator _creator;
        ThumbnailGrid _tgrid;
        string _directory;
        string _displayFilename;
        string _outTemplate;
        int    _nFiles;
        string _stats;
        TimeSpan _duration;
        ThumbnailPage _thumbnailPage;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ThumbnailWriter"/> class.
        /// </summary>
        /// <param name="creator">The <see cref="ThumbnailCreator"/>
        /// (only used to get a jpeg compression encoder).</param>
        /// <param name="tgrid">The <see cref="ThumbnailGrid"/>.</param>
        /// <param name="directory">The directory to write the thumbnail page.</param>
        /// <param name="displayFilename">The display name of the <see cref="AVFileSet"/>
        /// from which the thumbnails are generated.</param>
        /// <param name="outTemplate">The template used to generate page filename.</param>
        /// <param name="nFiles">The number of files in set (>0 for multi-part videos).</param>
        /// <param name="stats">The stats of the <see cref="AVFileSet"/> to display 
        /// in header.</param>
        /// <param name="duration">The duration of the <see cref="AVFileSet"/>.</param>
        public ThumbnailWriter (ThumbnailCreator creator,
                                ThumbnailGrid tgrid,
                                string directory,
                                string displayFilename,
                                string outTemplate,
                                int nFiles,
                                string stats,
                                TimeSpan duration)
            {
            this._creator = creator;
            this._tgrid = tgrid;
            this._directory = directory;
            this._displayFilename = displayFilename;
            this._outTemplate = outTemplate;
            this._nFiles = nFiles;
            this._stats = stats;
            this._duration = duration;

            _thumbnailPage = null;
            }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Add a <see cref="System.Drawing.Bitmap"/> thumbnail.
        /// </summary>
        /// <param name="thumbnail">The <see cref="System.Drawing.Bitmap"/> 
        /// thumbnail.</param>
        /// <param name="time">The <see cref="TimeSpan">time</see> the thumbnail was
        /// captured.</param>
        /// <param name="highlight">if set to <c>true</c> highlight thumbnail border.</param>
        /// <param name="fileNum">The file number
        /// (>0 for multi-file <see cref="AVFileSet"/>s).</param>
        public void Add (System.Drawing.Bitmap thumbnail, TimeSpan time, bool highlight, int fileNum)
            {
            if (_thumbnailPage == null)
                _thumbnailPage = CreateThumbnailPage (time);

            _thumbnailPage.Add (thumbnail, time, fileNum, TimeSpan.Zero, highlight);

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
            }

        /// <summary>
        /// Creates an overview thumbnail page.
        /// </summary>
        /// <param name="time">The <see cref="TimeSpan">time</see> of the first thumbnail 
        /// on page.</param>
        /// <returns>new <see cref="ThumbnailPage"/>.</returns>
        private ThumbnailPage CreateThumbnailPage (TimeSpan time)
            {
            string filename = _outTemplate;
            filename = System.IO.Path.Combine (_directory, filename);

            ThumbnailPage page = new ThumbnailPage (_creator, _tgrid,
                                                    _displayFilename, filename, _nFiles,
                                                    time, 
                                                    -1, 
                                                    _duration, 1,
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
                Console.Write("\b\b\b\b\b\b      \b\b\b\b\b\b");
                //Console.WriteLine ("");
                }
            }
        #endregion Methods
        }
    }
