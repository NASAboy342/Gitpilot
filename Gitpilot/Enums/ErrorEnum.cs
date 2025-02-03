
namespace Gitpilot.Enums
{
    public enum ErrorEnum
    {
        Sucess = 0,
        GeneralError = 1,
        RepositoryNotFound = 2,
        CommitNotFound = 3,
        BranchNotFound = 4,
        Conflict = 5,
        UnknownError = 6,
        NoChangesFound = 7,
        NoChangesToCommit = 8,
        NoChangesToPush = 9,
        NoChangesToPull = 10,
        NoChangesToMerge = 11,
        NoChangesToDelete = 12,
        NoChangesToCreate = 13,
        NoChangesToRename = 14,
        NoChangesToAmend = 15,
        NoChangesToStash = 16,
        NoChangesToPop = 17,
        NoChangesToMergeBase = 18,
        CacheReloadFailed = 19,
    }
}
