using System;
using System.IO;

namespace refs
{
    public static class RefsPrinter
    {
        private static string _gitPath = $@".{Path.DirectorySeparatorChar}.git";
        private static string _gitDirectoryPath;

        public static void PrintRefs(string gitDirectoryPath = "")
        {
            _gitDirectoryPath = gitDirectoryPath;
            _gitPath = gitDirectoryPath + _gitPath;
            if (!IsGitDirectoryExists()) throw new Exception("Cannot find \".git\" catalog.");
            if (!IsGitRefsDirectoryExists())
                throw new Exception("Cannot find \"refs\" catalog in \".git\".");

            ProcessHeads();
            ProcessRemotes();
            ProcessTags();
            ProcessPackedRefs();
        }

        private static void ProcessPackedRefs()
        {
            var packedRefsPath =
                $@"{_gitDirectoryPath}.{Path.DirectorySeparatorChar}.git{Path.DirectorySeparatorChar}packed-refs";
            if (!File.Exists(packedRefsPath)) return;
            using (var streamReader = new StreamReader(packedRefsPath))
            {
                while (!streamReader.EndOfStream)
                {
                    var content = streamReader.ReadLine();
                    if (content is null || content.Length < 1) continue;
                    switch (content[0])
                    {
                        case '#':
                        //TODO:need process referenced objects?
                        case '^':
                            continue;
                        default:
                        {
                            var contentAndName = content.Split(' ');
                            Console.WriteLine(contentAndName[1] + "\t" + contentAndName[0]);
                            break;
                        }
                    }
                }
            }
        }

        private static void ProcessTags()
        {
            var tagsPath = $"{_gitPath}{Path.DirectorySeparatorChar}refs{Path.DirectorySeparatorChar}tags";
            if (!Directory.Exists(tagsPath)) return;
            var files = Directory.GetFiles(tagsPath);

            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                var tagsName = file.Replace(_gitPath, "");
                Console.WriteLine(tagsName + "\t" + content.Replace("\n", ""));
            }
        }

        private static void ProcessRemotes()
        {
            var remotesPath = _gitPath + "" + Path.DirectorySeparatorChar + "refs" + Path.DirectorySeparatorChar + "remotes";
            if (!Directory.Exists(remotesPath)) return;
            var remotes = Directory.GetDirectories(remotesPath);
            foreach (var remote in remotes)
            {
                var files = Directory.GetFiles(remote);
                foreach (var file in files)
                {
                    var content = File.ReadAllText(file);
                    var remoteName = file.Replace(_gitPath, "");
                    Console.WriteLine(remoteName + "\t" + content.Replace("\n", ""));
                }
            }
        }

        private static bool IsGitRefsDirectoryExists()
        {
            return Directory.Exists(
                $@"{_gitDirectoryPath}.{Path.DirectorySeparatorChar}.git{Path.DirectorySeparatorChar}refs");
        }

        private static bool IsGitDirectoryExists()
        {
            return Directory.Exists($@"{_gitDirectoryPath}.{Path.DirectorySeparatorChar}.git");
        }

        private static void ProcessHeads()
        {
            var headsPath = $"{_gitPath}{Path.DirectorySeparatorChar}refs{Path.DirectorySeparatorChar}heads";
            if (!Directory.Exists(headsPath)) return;
            var files = Directory.GetFiles(headsPath);

            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                var refName = file.Replace(_gitPath, "");
                Console.WriteLine(refName + "\t" + content.Replace("\n", ""));
            }
        }
    }
}