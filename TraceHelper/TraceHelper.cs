using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TraceHelper
    {
    /// <summary>
    /// Outputs "clean" trace messages to the Console by skipping everything but the actual message.
    /// </summary>
    public class CleanConsole : System.Diagnostics.ConsoleTraceListener
        {
        /// <summary>
        /// Initializes a new instance of the <see cref="CleanConsole"/> class.
        /// </summary>
        public CleanConsole() : base()
            {
            }

        /// <summary>
        /// Writes trace information, a message, and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType"/> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="message">A message to write.</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode"/>
        ///   </PermissionSet>
        public override void TraceEvent (System.Diagnostics.TraceEventCache eventCache, string source, System.Diagnostics.TraceEventType eventType, int id, string message)
            {
            //base.TraceEvent (eventCache, source, eventType, id, message);
            if (Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null))
                WriteLine (message);
            }
        }

    /// <summary>
    /// Outputs "clean" trace messages to a file by skipping everything but the actual message.
    /// </summary>
    public class CleanTextWriter : System.Diagnostics.TextWriterTraceListener
        {
        /// <summary>
        /// Initializes a new instance of the <see cref="CleanTextWriter"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public CleanTextWriter(string filename) : base(filename)
            {
            }

        /// <summary>
        /// Writes trace information, a message, and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType"/> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="message">A message to write.</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode"/>
        ///   </PermissionSet>
        public override void TraceEvent (System.Diagnostics.TraceEventCache eventCache, string source, System.Diagnostics.TraceEventType eventType, int id, string message)
            {
            //base.TraceEvent (eventCache, source, eventType, id, message);
            if (Filter.ShouldTrace (eventCache, source, eventType, id, message, null, null, null))
                WriteLine (message);
            }
        }

    /// <summary>
    /// Class to make it easier to output diagnostic &amp; progress messages
    /// to the console and text files.
    /// </summary>
    /// <remarks>
    /// Example usage:
    /// <code lang="c#">
    /// using THelper = TraceHelper.TraceHelper;
    /// 
    /// THelper.AddTextLogger("MyProgram.log", "mainlogger");
    /// if (debugging)
    ///     THelper.SetConsoleLevel (System.Diagnostics.SourceLevels.Verbose);
    /// THelper.Information ("Just some information: {0}", somearg);
    /// THelper.Critical ("Some fatal error has occurred.");
    /// </code>
    /// </remarks>
    public static class TraceHelper
        {
        private static System.Diagnostics.TraceSource _tracer = 
                new System.Diagnostics.TraceSource ("Main",        
                    System.Diagnostics.SourceLevels.All);
        private static System.Diagnostics.ConsoleTraceListener _console;
        private static System.Diagnostics.SourceLevels _consoleLevels;

        private static string[] _progressAnimations = new string[]
            {
            "|",
            "/",
            "-",
            "\\"
            };
        private static int _animationIndex = 0;
        private static bool _animationStarted = false;

        static TraceHelper()
            {
            _tracer.Listeners.Remove ("Default");
            System.Diagnostics.Trace.AutoFlush = true;

            _console = new CleanConsole ();
            _console.Name = "console";
            _console.TraceOutputOptions = System.Diagnostics.TraceOptions.None;
            _console.Filter = new System.Diagnostics.EventTypeFilter (System.Diagnostics.SourceLevels.Information);
            _consoleLevels = System.Diagnostics.SourceLevels.Information;

            _tracer.Listeners.Add (_console);
            }

        /// <summary>
        /// Gets the singleton TracerHelper instance.
        /// </summary>
        /// <returns></returns>
        public static System.Diagnostics.TraceSource GetTracer ()
            {
            return _tracer;
            }

        /// <summary>
        /// Sets the Console message level.
        /// </summary>
        /// <param name="level">The Console message level.</param>
        public static void SetConsoleLevel(System.Diagnostics.SourceLevels level)
            {
            _console.Filter = new System.Diagnostics.EventTypeFilter(level);
            _consoleLevels = level;
            }


        /// <summary>
        /// Gets the Console message levels.
        /// </summary>
        public static System.Diagnostics.SourceLevels ConsoleLevels
            {
            get { return _consoleLevels; }
            }

        /// <summary>
        /// Adds logging to a file.
        /// </summary>
        /// <param name="filename">The name of the file to log to.</param>
        /// <param name="name">The name of the text file logger.</param>
        public static void AddTextLogger(string filename, string name)
            {
            CleanTextWriter _mainlogger = new CleanTextWriter (filename);

            _mainlogger.Name = name;
            _mainlogger.Filter = new System.Diagnostics.EventTypeFilter (System.Diagnostics.SourceLevels.All);
            _tracer.Listeners.Add (_mainlogger);
            }

        /// <summary>
        /// Removes a logger.
        /// </summary>
        /// <param name="name">The name of the text file logger to remove.</param>
        public static void RemoveTextLogger(string name)
            {
            _tracer.Listeners[name].Close ();
            _tracer.Listeners.Remove (name);
            }

        /// <summary>
        /// Gets the logger associated with <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the logger to get.</param>
        /// <returns></returns>
        public static System.Diagnostics.TraceListener GetLogger(string name)
            {
            return _tracer.Listeners[name];
            }

        /// <summary>
        /// Sets a logger message level.
        /// </summary>
        /// <param name="name">The name of the logger whose message level to set.</param>
        /// <param name="level">The logger message level.</param>
        public static void SetLoggerLevel (string name,
                                           System.Diagnostics.SourceLevels level)
            {
            _tracer.Listeners[name].Filter = new System.Diagnostics.EventTypeFilter (level);
            }

        /// <summary>
        /// Outputs a Debug level blank line.
        /// </summary>
        public static void Debug ()
            {
            _tracer.TraceEvent (System.Diagnostics.TraceEventType.Verbose, 0, "");
            }

        /// <summary>
        /// Outputs a Debug level message.
        /// </summary>
        /// <param name="message">The message to output.</param>
        public static void Debug (string message)
            {
            _tracer.TraceEvent (System.Diagnostics.TraceEventType.Verbose, 0, message);
            }

        /// <summary>
        /// Outputs a Debug level message with string formatting.
        /// </summary>
        /// <param name="format">The format string of the message to output.</param>
        /// <param name="args">The args of the message.</param>
        public static void Debug (string format, params object[] args)
            {
            string message = String.Format (format, args);
            _tracer.TraceEvent (System.Diagnostics.TraceEventType.Verbose, 0, message);
            }

        /// <summary>
        /// Outputs an Information level blank line.
        /// </summary>
        public static void Information ()
            {
            _tracer.TraceEvent (System.Diagnostics.TraceEventType.Information, 0, "");
            }

        /// <summary>
        /// Outputs an Information level message.
        /// </summary>
        /// <param name="message">The message to output.</param>
        public static void Information (string message)
            {
            _tracer.TraceEvent (System.Diagnostics.TraceEventType.Information, 0, message);
            }

        /// <summary>
        /// Outputs an Information level message with string formatting.
        /// </summary>
        /// <param name="format">The format string of the message to output.</param>
        /// <param name="args">The args of the message.</param>
        public static void Information (string format, params object[] args)
            {
            string message = String.Format (format, args);
            _tracer.TraceEvent (System.Diagnostics.TraceEventType.Information, 0, message);
            }

        /// <summary>
        /// Outputs a Warning level blank line.
        /// </summary>
        public static void Warning ()
            {
            _tracer.TraceEvent (System.Diagnostics.TraceEventType.Warning, 0, "");
            }

        /// <summary>
        /// Outputs a Warning level message.
        /// </summary>
        /// <param name="message">The message to output.</param>
        public static void Warning (string message)
            {
            _tracer.TraceEvent (System.Diagnostics.TraceEventType.Warning, 0, message);
            }

        /// <summary>
        /// Outputs a Warning level message with string formatting.
        /// </summary>
        /// <param name="format">The format string of the message to output.</param>
        /// <param name="args">The args of the message.</param>
        public static void Warning (string format, params object[] args)
            {
            string message = String.Format (format, args);
            _tracer.TraceEvent (System.Diagnostics.TraceEventType.Warning, 0, message);
            }

        /// <summary>
        /// Outputs an Error level blank line.
        /// </summary>
        public static void Error ()
            {
            _tracer.TraceEvent (System.Diagnostics.TraceEventType.Error, 0, "");
            }

        /// <summary>
        /// Outputs an Error level message.
        /// </summary>
        /// <param name="message">The message to output.</param>
        public static void Error (string message)
            {
            _tracer.TraceEvent (System.Diagnostics.TraceEventType.Error, 0, message);
            }

        /// <summary>
        /// Outputs an Error level message with string formatting.
        /// </summary>
        /// <param name="format">The format string of the message to output.</param>
        /// <param name="args">The args of the message.</param>
        public static void Error (string format, params object[] args)
            {
            string message = String.Format (format, args);
            _tracer.TraceEvent (System.Diagnostics.TraceEventType.Error, 0, message);
            }

        /// <summary>
        /// Outputs a Critical level blank line.
        /// </summary>
        public static void Critical ()
            {
            _tracer.TraceEvent (System.Diagnostics.TraceEventType.Critical, 0, "");
            }

        /// <summary>
        /// Outputs a Critical level message.
        /// </summary>
        /// <param name="message">The message to output.</param>
        public static void Critical (string message)
            {
            _tracer.TraceEvent (System.Diagnostics.TraceEventType.Critical, 0, message);
            }

        /// <summary>
        /// Outputs a Critical level message with string formatting.
        /// </summary>
        /// <param name="format">The format string of the message to output.</param>
        /// <param name="args">The args of the message.</param>
        public static void Critical (string format, params object[] args)
            {
            string message = String.Format (format, args);
            _tracer.TraceEvent (System.Diagnostics.TraceEventType.Critical, 0, message);
            }

        /// <summary>
        /// Resets the progress "animation" state.
        /// </summary>
        public static void ResetProgress ()
            {
            _animationStarted = false;
            _animationIndex = 0;
            }

        /// <summary>
        /// Gets the next progress "animation" string.
        /// </summary>
        /// <returns>The next progress "animation" string</returns>
        public static string GetNextProgressStr ()
            {
            string s = _progressAnimations[_animationIndex];
            if (_animationStarted)
                //s = "\b" + s;
            _animationStarted = true;

            _animationIndex++;
            if (_animationIndex >= _progressAnimations.Length)
                _animationIndex = 0;
            return s;
            }

        /// <summary>
        /// Gets the final progress "animation" string.
        /// </summary>
        /// <returns>The final progress "animation" string</returns>
        public static string GetLastProgressStr ()
            {
            string s = "";
            if (_animationStarted)
                //s = "\b \b";
            ResetProgress ();

            return s;
            }
        }
    }
