using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSEEncoder = Microsoft.Expression.Encoder;
using THelper = TraceHelper.TraceHelper;

namespace ThumbnailUtils
    {
    /// <summary>
    /// A List of <see cref="Microsoft.Expression.Encoder.AudioVideoFile"/>s
    /// </summary>
    public class AVFileSet : System.Collections.Generic.IEnumerable<MSEEncoder.AudioVideoFile>
        {
        #region Static Methods        
        /// <summary>
        /// Index Progess handler callback routine,
        /// </summary>
        /// <param name="src">The src object.</param>
        /// <param name="eArgs">The <see cref="Microsoft.Expression.Encoder.IndexProgressEventArgs"/> instance containing the event data.</param>
        public static void IndexProgessHandler (object src, MSEEncoder.IndexProgressEventArgs eArgs)
            {
            Console.Write ("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b");
            Console.Write ("               ");
            Console.Write ("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b");
            Console.Write (String.Format("{0,3:D}% Indexing", (int) (eArgs.Progress*100)));
            }

        /// <summary>
        /// Gets the default <see cref="Microsoft.Expression.Encoder.VideoStreamData"/> of a
        /// <see cref="Microsoft.Expression.Encoder.AudioVideoFile"/>.
        /// </summary>
        /// <param name="avFile">The <see cref="Microsoft.Expression.Encoder.AudioVideoFile"/>.</param>
        /// <returns>A <see cref="Microsoft.Expression.Encoder.VideoStreamData"/></returns>
        public static MSEEncoder.VideoStreamData GetVideoStream (MSEEncoder.AudioVideoFile avFile)
            {
            foreach (MSEEncoder.VideoStreamData videoStream in avFile.VideoStreams)
                if (videoStream.DefaultStream)
                    return videoStream;
            return null;
            }

        /// <summary>
        /// Gets the duration of the default 
        /// <see cref="Microsoft.Expression.Encoder.VideoStreamData"/>.
        /// </summary>
        /// <param name="avFile">The 
        /// <see cref="Microsoft.Expression.Encoder.AudioVideoFile"/>.</param>
        /// <returns><see cref="TimeSpan">Duration</see> of the default 
        /// <see cref="Microsoft.Expression.Encoder.VideoStreamData"/>.</returns>
        public static TimeSpan GetVideoDuration (MSEEncoder.AudioVideoFile avFile)
            {
            MSEEncoder.VideoStreamData videoStream = AVFileSet.GetVideoStream(avFile);
            if (videoStream != null)
                return videoStream.Duration;
            else
                return TimeSpan.Zero;
            }

        /// <summary>
        /// Gets the size of the default
        /// <see cref="Microsoft.Expression.Encoder.VideoStreamData"/>.
        /// </summary>
        /// <param name="avFile">The <see cref="Microsoft.Expression.Encoder.AudioVideoFile"/>.</param>
        /// <returns><see cref="System.Drawing.Size" /> of the default 
        /// <see cref="Microsoft.Expression.Encoder.VideoStreamData"/>.</returns>
        public static System.Drawing.Size GetVideoSize (MSEEncoder.AudioVideoFile avFile)
            {
            MSEEncoder.VideoStreamData videoStream = AVFileSet.GetVideoStream (avFile);
            if (videoStream != null)
                return videoStream.VideoSize;
            else
                return new System.Drawing.Size ();
            }

        /// <summary>
        /// Gets the aspect ratio of the default
        /// <see cref="Microsoft.Expression.Encoder.VideoStreamData"/>.
        /// </summary>
        /// <param name="avFile">The <see cref="Microsoft.Expression.Encoder.AudioVideoFile"/>.</param>
        /// <returns>The aspect ratio of the default 
        /// <see cref="Microsoft.Expression.Encoder.VideoStreamData"/>.</returns>
        public static double GetAspectRatio (MSEEncoder.AudioVideoFile avFile)
            {
            MSEEncoder.VideoStreamData videoStream = AVFileSet.GetVideoStream (avFile);
            if (videoStream != null)
                return (double) videoStream.AspectRatio.Width / videoStream.AspectRatio.Height;
            else
                return 0.0;
            }

        /// <summary>
        /// Gets the default <see cref="Microsoft.Expression.Encoder.AudioStreamData"/> of a
        /// <see cref="Microsoft.Expression.Encoder.AudioVideoFile"/>.
        /// </summary>
        /// <param name="avFile">The <see cref="Microsoft.Expression.Encoder.AudioVideoFile"/>.</param>
        /// <returns>A <see cref="Microsoft.Expression.Encoder.AudioStreamData"/></returns>
        public static MSEEncoder.AudioStreamData GetAudioStream (MSEEncoder.AudioVideoFile avFile)
            {
            foreach (MSEEncoder.AudioStreamData audioStream in avFile.AudioStreams)
                if (audioStream.DefaultStream)
                    return audioStream;
            return null;
            }

        /// <summary>
        /// Gets the duration of the default
        /// <see cref="Microsoft.Expression.Encoder.AudioStreamData"/>.
        /// </summary>
        /// <param name="avFile">The <see cref="Microsoft.Expression.Encoder.AudioVideoFile"/>.</param>
        /// <returns><see cref="TimeSpan">Duration</see> of the default 
        /// <see cref="Microsoft.Expression.Encoder.AudioStreamData"/>.</returns>
        public static TimeSpan GetAudioDuration (MSEEncoder.AudioVideoFile avFile)
            {
            MSEEncoder.AudioStreamData audioStream = AVFileSet.GetAudioStream (avFile);
            if (audioStream != null)
                return audioStream.Duration;
            else
                return TimeSpan.Zero;
            }

        /// <summary>
        /// Gets the number of channels of the default
        /// <see cref="Microsoft.Expression.Encoder.AudioStreamData"/>.
        /// </summary>
        /// <param name="avFile">The <see cref="Microsoft.Expression.Encoder.AudioVideoFile"/>.</param>
        /// <returns>
        /// The number of audio channels.
        /// </returns>
        public static int GetAudioChannels (MSEEncoder.AudioVideoFile avFile)
            {
            MSEEncoder.AudioStreamData audioStream = AVFileSet.GetAudioStream (avFile);
            if (audioStream != null)
                return audioStream.Channels;
            else
                return 0;
            }

        /// <summary>
        /// Gets the audio stream stats.
        /// </summary>
        /// <param name="avFile">The <see cref="Microsoft.Expression.Encoder.AudioVideoFile"/>.</param>
        /// <returns>Audio channel stats as a <see cref="string"/>.</returns>
        private static string GetAudioStreamStats (MSEEncoder.AudioVideoFile avFile)
            {
            StringBuilder sb = new StringBuilder("[");
            foreach (MSEEncoder.AudioStreamData audioStream in avFile.AudioStreams)
                {
                if (sb.Length > 1)
                    sb.Append (",");
                sb.AppendFormat("{0}ch", audioStream.Channels);
                if (audioStream.DefaultStream)
                    sb.Append ("*");
                }
            sb.Append ("]");
            return sb.ToString ();
            }
        #endregion Static Methods

        #region Fields
        private List<MSEEncoder.AudioVideoFile> _avFiles = new List<MSEEncoder.AudioVideoFile>();

        private TimeSpan _totalDuration = new TimeSpan ();
        private long _totalFileSize = 0L;

        private System.Text.RegularExpressions.Regex _displaynameRE = 
            new System.Text.RegularExpressions.Regex (
                @"^(.*?)[- ._]+\w*?\w\.\w+$",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        private System.Text.RegularExpressions.Regex _vobIndexRE = 
            new System.Text.RegularExpressions.Regex (
                @"^vts_(\d+)_\d+\.vob$",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        #endregion Fields

        #region Properties
        /// <summary>
        /// Gets the <see cref="TimeSpan">total duration</see> of the 
        /// <see cref="Microsoft.Expression.Encoder.AudioVideoFile"/>s in the
        /// <see cref="AVFileSet"/>.
        /// </summary>
        public TimeSpan TotalDuration
            {
            get { return _totalDuration; }
            }

        /// <summary>
        /// Gets the total size of the 
        /// <see cref="Microsoft.Expression.Encoder.AudioVideoFile"/>s in the
        /// <see cref="AVFileSet"/>.
        /// </summary>
        public long TotalFileSize
            {
            get { return _totalFileSize; }
            }

        /// <summary>
        /// Gets the number of 
        /// <see cref="Microsoft.Expression.Encoder.AudioVideoFile"/>s in the
        /// <see cref="AVFileSet"/>.
        /// </summary>
        public int Count 
            {
            get { return _avFiles.Count; }
            }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        /// </returns>
        public System.Collections.Generic.IEnumerator<MSEEncoder.AudioVideoFile> GetEnumerator ()
            {
            return this._avFiles.GetEnumerator ();
            }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
            {
            return this.GetEnumerator ();
            }

        /// <summary>
        /// Gets the <see cref="Microsoft.Expression.Encoder.AudioVideoFile"/> with the specified i.
        /// </summary>
        public MSEEncoder.AudioVideoFile this[int i]
            {
            get { return _avFiles[i]; }
            }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Adds the specified 
        /// <see cref="Microsoft.Expression.Encoder.AudioVideoFile"/>.
        /// </summary>
        /// <param name="avFile">The
        /// <see cref="Microsoft.Expression.Encoder.AudioVideoFile"/> to add.</param>
        /// <returns>Current Number of
        /// <see cref="Microsoft.Expression.Encoder.AudioVideoFile"/>s in the
        /// <see cref="AVFileSet"/>.</returns>
        public int Add(MSEEncoder.AudioVideoFile avFile)
            {
            _avFiles.Add(avFile);
            _totalDuration += avFile.VideoStreams[0].Duration;
            _totalFileSize += avFile.FileSize;

            return Count;
            }

        /// <summary>
        /// Updates the total duration and size of the <see cref="AVFileSet"/>.
        /// </summary>
        public void UpdateTotals()
            {
            _totalDuration = new TimeSpan (0, 0, 0);
            _totalFileSize = 0L;

            foreach (MSEEncoder.AudioVideoFile avFile in this)
                {
                _totalDuration += avFile.VideoStreams[0].Duration;
                _totalFileSize += avFile.FileSize;
                }
            }

        /// <summary>
        /// Gets the name to use when "displaying" <see cref="AVFileSet"/> information.
        /// </summary>
        /// <param name="outputDirectory">The output directory.</param>
        /// <returns></returns>
        public string GetDisplayName (string outputDirectory)
            {
            string displayName = "";
            if (Count < 1)
                return displayName;

            string filename = this[0].FileName;
            string extension = System.IO.Path.GetExtension (filename).ToLower();
            string basename = System.IO.Path.GetFileName (filename);

            if (extension == ".vob")
                {
                displayName = System.IO.Path.GetFileNameWithoutExtension (
                    System.IO.Path.GetFullPath (outputDirectory));
                if (Count > 1)
                    {
                    System.Text.RegularExpressions.Match match = _vobIndexRE.Match (basename);
                    if (match.Success)
                        displayName += "_-_vts_" + match.Groups[1].Value;
                    else
                        displayName += "_-_vts_xx";

                    displayName += ".dvd";
                    }
                else
                    displayName += "_" + basename;
                }
            else
                {
                displayName = basename;
                if (Count > 1)
                    {
                    System.Text.RegularExpressions.Match match = _displaynameRE.Match (basename);
                    if (match.Success)
                        displayName = match.Groups[1].Value + "_MULTI" + extension;
                    }
                }

            return displayName;
            }

        #endregion Methods
        }
    }
