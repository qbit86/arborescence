namespace Ubiquitous.Workbench
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;

    public static class IndexedGraphs
    {
        private static readonly UTF8Encoding s_utf8Encoding = new UTF8Encoding(false, false);

        public static TextReader GetTextReader(string shortName)
        {
            if (shortName == null)
                throw new ArgumentNullException(nameof(shortName));

            Stream stream = typeof(IndexedGraphs).GetTypeInfo().Assembly
                .GetManifestResourceStream($"Ubiquitous.Workbench.IndexedGraphs.{shortName}.gv");
            if (stream == null)
                return TextReader.Null;

            return new StreamReader(stream, s_utf8Encoding, false);
        }
    }
}
