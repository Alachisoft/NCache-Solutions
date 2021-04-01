using System;
using System.Configuration;
using Native.StackExchange.Redis.Sample.BasicCacheOperations;
using Native.StackExchange.Redis.Sample.BulkCacheOperations;
using Native.StackExchange.Redis.Sample.CacheConnectionOperations;
using Native.StackExchange.Redis.Sample.CacheContainsOperations;
using Native.StackExchange.Redis.Sample.DatastructuresOperations.ListOperations;
using Native.StackExchange.Redis.Sample.DatastructuresOperations.SetsOperations;
using Native.StackExchange.Redis.Sample.DatastructuresultOperations.SetsOperations;
using Native.StackExchange.Redis.Sample.ExpirationOperations;
using Native.StackExchange.Redis.Sample.KeyOperations;
using Native.StackExchange.Redis.Sample.ObjectToOrFroByteArrayOperations;
using StackExchange.Redis;


namespace Native.StackExchange.Redis.Sample
{
    class Program
    {
        public static int successfulTests = 0;
        public static int failedTests = 0;
        public static IDatabase db;
        public static ConnectionMultiplexer redis;
        public static string myObjectForCaching = "This is my Object";

        static void Main(string[] args)
        {
            InitializeClient();
            RunAllTests();

            Console.WriteLine($"Test completed successfully.\nSuccessful Tests: {successfulTests}\nFailed Tests: {failedTests}\nPress any key to exit...");
            Console.ReadKey();
        }

        private static void InitializeClient()
        {
            string serverIP = ConfigurationManager.AppSettings["ServerIP"];

            var options = ConfigurationOptions.Parse(serverIP);
            options.ConnectRetry = 5;
            options.AllowAdmin = true;

            redis = ConnectionMultiplexer.Connect(options);
            db = redis.GetDatabase();
        }

        private static void RunAllTests()
        {
            RunObjectToOrFroByteArrayOperations();
            RunKeyOperations();
            RunExpirationOperations();
            RunCacheContainsOperations();
            RunCacheConnectionOperations();
            RunBulkCacheOperationsTests();
            RunBasicCacheOperationsTests();
            RunDatastructuresTests();
        }

        private static void RunObjectToOrFroByteArrayOperations()
        {
            RunKeyDumpTests();
            RunKeyRestoreTests();
        }

        private static void RunKeyDumpTests()
        {
            KeyDumpTests.GetByteArrayForExistingObject();
            KeyDumpTests.GetByteArrayForExistingObjectAsync();
            KeyDumpTests.GetByteArrayForNonExistingObject();
            KeyDumpTests.GetByteArrayForNonExistingObjectAsync();
        }

        private static void RunKeyRestoreTests()
        {
            KeyRestoreTests.RestoreExistingKey();
            KeyRestoreTests.RestoreExistingKeyAsync();
            KeyRestoreTests.RestoreNonExistingKey();
            KeyRestoreTests.RestoreNonExistingKeyAsync();
        }

        private static void RunKeyOperations()
        {
            RunKeyTouchTests();
            RunKeyRenameTests();
            RunKeyRandomTests();
            RunBulkKeyTouchTests();
        }

        private static void RunKeyTouchTests()
        {
            KeyTouchTests.TouchExistingKey();
            KeyTouchTests.TouchExistingKeyAsync();
            KeyTouchTests.TouchNonExistingKey();
            KeyTouchTests.TouchNonExistingKeyAsync();
        }

        private static void RunKeyRenameTests()
        {
            KeyRenameTests.RenameExistingKey();
            KeyRenameTests.RenameExistingKeyAsync();
            KeyRenameTests.RenameNonExistingKey();
            KeyRenameTests.RenameNonExistingKeyAsync();
        }

        private static void RunKeyRandomTests()
        {
            KeyRandomTests.GetRandomKeyFromEmptyCache();
            KeyRandomTests.GetRandomKeyFromEmptyCacheAsync();
            KeyRandomTests.GetRandomKeyFromNonEmptyCache();
            KeyRandomTests.GetRandomKeyFromNonEmptyCacheAsync();
        }

        private static void RunBulkKeyTouchTests()
        {
            BulkKeyTouchTests.TouchExistingKeysBulk();
            BulkKeyTouchTests.TouchExistingKeysBulkAsync();
            BulkKeyTouchTests.TouchNonExistingKeysBulk();
            BulkKeyTouchTests.TouchNonExistingKeysBulkAsync();
        }

        private static void RunExpirationOperations()
        {
            RunKeyPersistsTests();
            RunKeyExpireTests();
        }

        private static void RunKeyPersistsTests()
        {
            KeyPersistsTests.PersistExistingKey();
            KeyPersistsTests.PersistExistingKeyAsync();
            KeyPersistsTests.PersistNonExistingKey();
            KeyPersistsTests.PersistNonExistingKeyAsync();
        }

        private static void RunKeyExpireTests()
        {
            KeyExpireTests.SetDateTimeExpirationForExistingItem();
            KeyExpireTests.SetDateTimeExpirationForExistingItemAsync();
            KeyExpireTests.SetDateTimeExpirationForNonExistingItem();
            KeyExpireTests.SetDateTimeExpirationForNonExistingItemAsync();
            KeyExpireTests.SetTimeSpanExpirationForExistingItem();
            KeyExpireTests.SetTimeSpanExpirationForExistingItemAsync();
            KeyExpireTests.SetTimeSpanExpirationForNonExistingItem();
            KeyExpireTests.SetTimeSpanExpirationForNonExistingItemAsync();
        }

        private static void RunCacheContainsOperations()
        {
            RunKeyExistsTests();
            RunBulkKeyExistsTests();
        }

        private static void RunKeyExistsTests()
        {
            KeyExistsTests.ContainsExistingKey();
            KeyExistsTests.ContainsExistingKeyAsync();
            KeyExistsTests.ContainsNonExistingKey();
            KeyExistsTests.ContainsNonExistingKeyAsync();
        }

        private static void RunBulkKeyExistsTests()
        {
            BulkKeyExistsTests.ContainsExistingKeysBulk();
            BulkKeyExistsTests.ContainsExistingKeysBulkAsync();
            BulkKeyExistsTests.ContainsNonExistingKeysBulk();
            BulkKeyExistsTests.ContainsNonExistingKeysBulkAsync();
        }

        private static void RunCacheConnectionOperations()
        {
            CacheConnectionTests.ChechCacheConnection();
        }

        private static void RunBulkCacheOperationsTests()
        {
            RunBulkStringSetTests();
            RunBulkKeyDeleteTests();
        }

        private static void RunBulkStringSetTests()
        {
            BulkStringSetTests.AddExistingItemsBulk();
            BulkStringSetTests.AddExistingItemsBulkAsync();
            BulkStringSetTests.AddNonExistingItemsBulk();
            BulkStringSetTests.AddNonExistingItemsBulkAsync();
        }

        private static void RunBulkKeyDeleteTests()
        {
            BulkKeyDeleteTests.DeleteExistingKeysBulk();
            BulkKeyDeleteTests.DeleteExistingKeysBulkAsync();
            BulkKeyDeleteTests.DeleteNonExistingKeysBulk();
            BulkKeyDeleteTests.DeleteNonExistingKeysBulkAsync();
        }

        private static void RunDatastructuresTests()
        {
            RunSetsDataStructuresTests();
            RunListDatastructuresTests();
        }

        private static void RunBasicCacheOperationsTests()
        {
            RunStringLengthTests();
            RunStringGetSetTests();
            RunStringGetRangeTests();
            RunStringIncrementTests();
            RunStringDecrementTests();
            RunStringSetTests();
            RunStringGetTests();
            RunKeyDeleteTests();
            RunStringAppendTests();
        }

        private static void RunStringLengthTests()
        {
            StringLengthTests.GetExistingStringLength();
            StringLengthTests.GetExistingStringLengthAsync();
            StringLengthTests.GetNonExistingStringLength();
            StringLengthTests.GetNonExistingStringLengthAsync();
            StringLengthTests.GetNonStringLength();
            StringLengthTests.GetNonStringLengthAsync();
        }

        private static void RunStringGetSetTests()
        {
            StringGetSetTests.GetExistingStringAndSetInt();
            StringGetSetTests.GetExistingStringAndSetIntAsync();
            StringGetSetTests.GetSetExistingKey();
            StringGetSetTests.GetSetExistingKeyAsync();
            StringGetSetTests.GetSetNonExistingKey();
            StringGetSetTests.GetSetNonExistingKeyAsync();
        }

        private static void RunStringGetRangeTests()
        {
            StringGetRangeTests.GetExistingStringRange();
            StringGetRangeTests.GetExistingStringRangeAsync();
            StringGetRangeTests.GetExistingStringRangeWithNegativeEnd();
            StringGetRangeTests.GetExistingStringRangeWithNegativeEndAsync();
            StringGetRangeTests.GetExistingStringRangeWithNegativeStart();
            StringGetRangeTests.GetExistingStringRangeWithNegativeStartAsync();
            StringGetRangeTests.GetExistingStringRangeWithOutOfBoundEnd();
            StringGetRangeTests.GetExistingStringRangeWithOutOfBoundEndAsync();
            StringGetRangeTests.GetExistingStringRangeWithOutOfBoundStart();
            StringGetRangeTests.GetExistingStringRangeWithOutOfBoundStartAsync();
            StringGetRangeTests.GetNonExistingStringRange();
            StringGetRangeTests.GetNonExistingStringRangeAsync();
        }

        private static void RunStringIncrementTests()
        {
            StringIncrementTests.IncrementExistingInValidString();
            StringIncrementTests.IncrementExistingInValidStringAsync();
            StringIncrementTests.IncrementExistingValidString();
            StringIncrementTests.IncrementExistingValidStringAsync();
            StringIncrementTests.IncrementExistingValidStringByDoubleValue();
            StringIncrementTests.IncrementExistingValidStringByDoubleValueAsync();
            StringIncrementTests.IncrementExistingValidStringByLongValue();
            StringIncrementTests.IncrementExistingValidStringByLongValueAsync();
            StringIncrementTests.IncrementNonExistingString();
            StringIncrementTests.IncrementNonExistingStringAsync();
        }

        private static void RunStringDecrementTests()
        {
            StringDecrementTests.DecrementExistingInValidString();
            StringDecrementTests.DecrementExistingInValidStringAsync();
            StringDecrementTests.DecrementExistingValidString();
            StringDecrementTests.DecrementExistingValidStringAsync();
            StringDecrementTests.DecrementExistingValidStringByDoubleValue();
            StringDecrementTests.DecrementExistingValidStringByDoubleValueAsync();
            StringDecrementTests.DecrementExistingValidStringByLongValue();
            StringDecrementTests.DecrementExistingValidStringByLongValueAsync();
            StringDecrementTests.DecrementNonExistingString();
            StringDecrementTests.DecrementNonExistingStringAsync();
        }

        private static void RunStringSetTests()
        {
            StringSetTests.AddExistingKeyValuePair();
            StringSetTests.AddExistingKeyValuePairAsync();
            StringSetTests.AddKeyValuePair();
            StringSetTests.AddKeyValuePairAsync();
        }

        private static void RunStringGetTests()
        {
            StringGetTests.GetExistingKey();
            StringGetTests.GetExistingKeyAsync();
            StringGetTests.GetNonExistingKey();
            StringGetTests.GetNonExistingKeyAsync();
        }

        private static void RunKeyDeleteTests()
        {
            KeyDeleteTests.DeleteExistingKey();
            KeyDeleteTests.DeleteExistingKeyAsync();
            KeyDeleteTests.DeleteNonExistingKey();
            KeyDeleteTests.DeleteNonExistingKeyAsync();
        }

        private static void RunStringAppendTests()
        {
            StringAppendTests.AppendEmptyStringToExistingString();
            StringAppendTests.AppendEmptyStringToExistingStringAsync();
            StringAppendTests.AppendEmptyStringToNonExistingString();
            StringAppendTests.AppendEmptyStringToNonExistingStringAsync();
            StringAppendTests.AppendToExistingInt();
            StringAppendTests.AppendToExistingIntAsync();
            StringAppendTests.AppendToExistingString();
            StringAppendTests.AppendToExistingStringAsync();
            StringAppendTests.AppendToNonExistingString();
            StringAppendTests.AppendToNonExistingStringAsync();
        }

        private static void RunSetsDataStructuresTests()
        {
            RunBulkSetRemoveTests();
            RunSetRemoveTests();
            RunBulkSetRandomMembersTests();
            RunSetRandomMemberTests();
            RunBulkSetPopTests();
            RunSetPopTests();
            RunSetMoveTests();
            RunSetMembersTests();
            RunSetLengthTests();
            RunSetContainsTests();
            RunSetCombineAndStoreTests();
            RunBulkSetCombineTests();
            RunSetCombineTests();
            RunBulkSetAddTests();
            RunSetAddTests();
        }

        private static void RunBulkSetRemoveTests()
        {
            BulkSetRemoveTests.RemoveExistingItemsBulkFromSet();
            BulkSetRemoveTests.RemoveExistingItemsBulkFromSetAsync();
            BulkSetRemoveTests.RemoveItemsBulkFromExistingEmptySet();
            BulkSetRemoveTests.RemoveItemsBulkFromExistingEmptySetAsync();
            BulkSetRemoveTests.RemoveItemsBulkFromNonExistingSet();
            BulkSetRemoveTests.RemoveItemsBulkFromNonExistingSetAsync();
            BulkSetRemoveTests.RemoveItemsBulkGreaterThanExistingItemsCountFromSet();
            BulkSetRemoveTests.RemoveItemsBulkGreaterThanExistingItemsCountFromSetAsync();
            BulkSetRemoveTests.RemoveNonExistingItemsBulkFromSet();
            BulkSetRemoveTests.RemoveNonExistingItemsBulkFromSetAsync();
        }

        private static void RunSetRemoveTests()
        {
            SetRemoveTests.RemoveExistingItemFromSet();
            SetRemoveTests.RemoveExistingItemFromSetAsync();
            SetRemoveTests.RemoveItemFromExistingEmptySet();
            SetRemoveTests.RemoveItemFromExistingEmptySetAsync();
            SetRemoveTests.RemoveItemFromNonExistingSet();
            SetRemoveTests.RemoveItemFromNonExistingSetAsync();
            SetRemoveTests.RemoveNonExistingItemFromSet();
            SetRemoveTests.RemoveNonExistingItemFromSetAsync();
        }

        private static void RunBulkSetRandomMembersTests()
        {
            BulkSetRandomMembersTests.GetRandomItemsBulkFromEmptySet();
            BulkSetRandomMembersTests.GetRandomItemsBulkFromEmptySetAsync();
            BulkSetRandomMembersTests.GetRandomItemsBulkFromNonEmptySet();
            BulkSetRandomMembersTests.GetRandomItemsBulkFromNonEmptySetAsync();
            BulkSetRandomMembersTests.GetRandomItemsBulkFromNonEmptySetWithCountGreaterThanSetCount();
            BulkSetRandomMembersTests.GetRandomItemsBulkFromNonEmptySetWithCountGreaterThanSetCountAsync();
            BulkSetRandomMembersTests.GetRandomItemsBulkFromNonExistingSet();
            BulkSetRandomMembersTests.GetRandomItemsBulkFromNonExistingSetAsync();
        }

        private static void RunSetRandomMemberTests()
        {
            SetRandomMemberTests.GetRandomItemFromEmptySet();
            SetRandomMemberTests.GetRandomItemFromNonEmptySet();
            SetRandomMemberTests.GetRandomItemFromNonEmptySetAsync();
            SetRandomMemberTests.GetRandomItemFromNonExistingSet();
            SetRandomMemberTests.PopItemFromEmptySetAsync();
            SetRandomMemberTests.PopItemFromNonExistingSetAsync();
        }

        private static void RunBulkSetPopTests()
        {
            BulkSetPopTests.PopItemsBulkFromEmptySet();
            BulkSetPopTests.PopItemsBulkFromEmptySetAsync();
            BulkSetPopTests.PopItemsBulkFromNonEmptySet();
            BulkSetPopTests.PopItemsBulkFromNonEmptySetAsync();
            BulkSetPopTests.PopItemsBulkFromNonEmptySetWithCountGreaterThanSetCount();
            BulkSetPopTests.PopItemsBulkFromNonEmptySetWithCountGreaterThanSetCountAsync();
            BulkSetPopTests.PopItemsBulkFromNonExistingSet();
            BulkSetPopTests.PopItemsBulkFromNonExistingSetAsync();
        }

        private static void RunSetPopTests()
        {
            SetPopTests.PopItemFromEmptySet();
            SetPopTests.PopItemFromEmptySetAsync();
            SetPopTests.PopItemFromNonEmptySet();
            SetPopTests.PopItemFromNonEmptySetAsync();
            SetPopTests.PopItemFromNonExistingSet();
            SetPopTests.PopItemFromNonExistingSetAsync();
        }

        private static void RunSetMoveTests()
        {
            SetMoveTests.MoveItemAmongExistingSets();
            SetMoveTests.MoveItemAmongExistingSetsAsync();
            SetMoveTests.MoveItemAmongNonExistingSets();
            SetMoveTests.MoveItemAmongNonExistingSetsAsync();
            SetMoveTests.MoveItemToNonExistingDestinationSet();
            SetMoveTests.MoveItemToNonExistingDestinationSetAsync();
            SetMoveTests.MoveItemFromNonExistingSourceSet();
            SetMoveTests.MoveItemFromNonExistingSourceSetAsync();
            SetMoveTests.MoveNonExistingItemToNonExistingDestinationSet();
            SetMoveTests.MoveNonExistingItemToNonExistingDestinationSetAsync();
        }

        private static void RunSetMembersTests()
        {
            SetMembersTests.GetMembersOfEmptySet();
            SetMembersTests.GetMembersOfEmptySetAsync();
            SetMembersTests.GetMembersOfExistingSet();
            SetMembersTests.GetMembersOfExistingSetAsync();
            SetMembersTests.GetMembersOfNonExistingSet();
            SetMembersTests.GetMembersOfNonExistingSetAsync();
        }

        private static void RunSetLengthTests()
        {
            SetLengthTests.GetLengthOfEmptySet();
            SetLengthTests.GetLengthOfEmptySetAsync();
            SetLengthTests.GetLengthOfExistingSet();
            SetLengthTests.GetLengthOfExistingSetAsync();
            SetLengthTests.GetLengthOfNonExistingSet();
            SetLengthTests.GetLengthOfNonExistingSetAsync();
        }

        private static void RunSetContainsTests()
        {
            SetContainsTests.CheckExistingItemInExistingSet();
            SetContainsTests.CheckExistingItemInExistingSetAsync();
            SetContainsTests.CheckItemInNonExistingSet();
            SetContainsTests.CheckItemInNonExistingSetAsync();
            SetContainsTests.CheckNonExistingItemInExistingSet();
            SetContainsTests.CheckNonExistingItemInExistingSetAsync();
        }

        private static void RunSetCombineAndStoreTests()
        {
            SetCombineAndStoreTests.CombineAndStoreExistingSets();
            SetCombineAndStoreTests.CombineAndStoreExistingSetsAsync();
            SetCombineAndStoreTests.CombineAndStoreNonExistingFirstSet();
            SetCombineAndStoreTests.CombineAndStoreNonExistingFirstSetAsync();
            SetCombineAndStoreTests.CombineAndStoreNonExistingSecondSet();
            SetCombineAndStoreTests.CombineAndStoreNonExistingSecondSetAsync();
            SetCombineAndStoreTests.CombineAndStoreNonExistingSets();
            SetCombineAndStoreTests.CombineAndStoreNonExistingSetsAsync();
        }

        private static void RunBulkSetCombineTests()
        {
            BulkSetCombineTests.CombineExistingBulkSets();
            BulkSetCombineTests.CombineExistingBulkSetsAsync();
            BulkSetCombineTests.CombineNonExistingBulkSets();
            BulkSetCombineTests.CombineNonExistingBulkSetsAsync();
        }

        private static void RunSetCombineTests()
        {
            SetCombineTests.CombineExistingSets();
            SetCombineTests.CombineExistingSetsAsync();
            SetCombineTests.CombineNonExistingFirstSet();
            SetCombineTests.CombineNonExistingFirstSetAsync();
            SetCombineTests.CombineNonExistingSecondSet();
            SetCombineTests.CombineNonExistingSecondSetAsync();
            SetCombineTests.CombineNonExistingSets();
            SetCombineTests.CombineNonExistingSetsAsync();
        }

        private static void RunBulkSetAddTests()
        {
            BulkSetAddTests.AddBulkInAlreadyExistingSet();
            BulkSetAddTests.AddBulkInAlreadyExistingSetAsync();
            BulkSetAddTests.AddBulkInNonExistingSet();
            BulkSetAddTests.AddBulkInNonExistingSetAsync();
            BulkSetAddTests.AddExistingBulkInSet();
            BulkSetAddTests.AddExistingBulkInSetAsync();
        }

        private static void RunSetAddTests()
        {
            SetAddTests.AddExistingItemInExistingSet();
            SetAddTests.AddExistingItemInExistingSetAsync();
            SetAddTests.AddInAlreadyExistingSet();
            SetAddTests.AddInAlreadyExistingSetAsync();
            SetAddTests.AddInNonExistingSet();
            SetAddTests.AddInNonExistingSetAsync();
        }

        private static void RunListDatastructuresTests()
        {
            RunListInsertAfterTests();
            RunListGetByIndexTests();
            RunListTrimTests();
            RunListSetByIndexTests();
            RunBulkRightPushTests();
            RunListRightPushTests();
            RunListRightPopLeftPushTests();
            RunListRightPopTests();
            RunListRemoveTests();
            RunListRangeTests();
            RunListLengthTests();
            RunBulkLeftPushTests();
            RunListLeftPushTests();
            RunListLeftPopTests();
            RunListInsertBeforeTests();
        }

        private static void RunListInsertAfterTests()
        {
            ListInsertAfterTests.InsertAfterExistingPivotAndList();
            ListInsertAfterTests.InsertAfterExistingPivotAndListAsync();
            ListInsertAfterTests.InsertAfterNonExistingPivotAndExistingList();
            ListInsertAfterTests.InsertAfterNonExistingPivotAndExistingListAsync();
            ListInsertAfterTests.InsertAfterNonExistingPivotAndList();
            ListInsertAfterTests.InsertAfterNonExistingPivotAndListAsync();
        }

        private static void RunListGetByIndexTests()
        {
            ListGetByIndexTests.GetExistingItemFromExistingList();
            ListGetByIndexTests.GetExistingItemFromExistingListAsync();
            ListGetByIndexTests.GetItemFromNonExistingList();
            ListGetByIndexTests.GetItemFromNonExistingListAsync();
            ListGetByIndexTests.GetNonExistingItemFromExistingList();
            ListGetByIndexTests.GetNonExistingItemFromExistingListAsync();
        }

        private static void RunListTrimTests()
        {
            ListTrimTests.TrimExistingList();
            ListTrimTests.TrimExistingListAsync();
            ListTrimTests.TrimExistingListWithInvalidStart();
            ListTrimTests.TrimExistingListWithInvalidStartAsync();
            ListTrimTests.TrimNonExistingList();
            ListTrimTests.TrimNonExistingListAsync();
        }

        private static void RunListSetByIndexTests()
        {
            ListSetByIndexTests.SetByIndexInExistingList();
            ListSetByIndexTests.SetByIndexInExistingListAsync();
            ListSetByIndexTests.SetByIndexInNonExistingList();
            ListSetByIndexTests.SetByIndexInNonExistingListAsync();
        }

        private static void RunBulkRightPushTests()
        {
            BulkListRightPushTests.RightPushBulkItemsInExistingList();
            BulkListRightPushTests.RightPushBulkItemsInNonExistingList();
            BulkListRightPushTests.RightPushItemInExistingListAsync();
            BulkListRightPushTests.RightPushItemInNonExistingListAsync();
        }

        private static void RunListRightPushTests()
        {
            ListRightPushTests.RightPushItemInExistingList();
            ListRightPushTests.RightPushItemInExistingListAsync();
            ListRightPushTests.RightPushItemInNonExistingList();
            ListRightPushTests.RightPushItemInNonExistingListAsync();
        }

        private static void RunListRightPopLeftPushTests()
        {
            ListRightPopLeftPushTests.RightPopLeftPushInExistingLists();
            ListRightPopLeftPushTests.RightPopLeftPushInExistingListsAsync();
            ListRightPopLeftPushTests.RightPopLeftPushInNonExistingDestinationList();
            ListRightPopLeftPushTests.RightPopLeftPushInNonExistingDestinationListAsync();
            ListRightPopLeftPushTests.RightPopLeftPushInNonExistingLists();
            ListRightPopLeftPushTests.RightPopLeftPushInNonExistingListsAsync();
            ListRightPopLeftPushTests.RightPopLeftPushInNonExistingSourceList();
            ListRightPopLeftPushTests.RightPopLeftPushInNonExistingSourceListAsync();
        }

        private static void RunListRightPopTests()
        {
            ListRightPopTests.RightPopFromExistingEmptyList();
            ListRightPopTests.RightPopFromExistingEmptyListAsync();
            ListRightPopTests.RightPopFromExistingList();
            ListRightPopTests.RightPopFromExistingListAsync();
            ListRightPopTests.RightPopFromNonExistingList();
            ListRightPopTests.RightPopFromNonExistingListAsync();
        }

        private static void RunListRemoveTests()
        {
            ListRemoveTests.RemoveExistingItemFromExistingList();
            ListRemoveTests.RemoveNonExistingItemFromExistingList();
            ListRemoveTests.RemoveItemFromNonExistingList();
            ListRemoveTests.RemoveItemFromListPassingCount();
            ListRemoveTests.RemoveExistingItemFromExistingListAsync();
            ListRemoveTests.RemoveItemFromListPassingCountAsync();
            ListRemoveTests.RemoveItemFromNonExistingListAsync();
            ListRemoveTests.RemoveNonExistingItemFromExistingListAsync();
        }

        private static void RunListRangeTests()
        {
            ListRangeTests.GetExistingListRangeByPassingJustListKey();
            ListRangeTests.GetNonExistingListRangeByPassingJustListKey();
            ListRangeTests.GetExistingListRangeByPassingValidArguments();
            ListRangeTests.GetExistingListRangeByPassingInValidArguments();
            ListRangeTests.GetExistingListRangeByPassingInValidArgumentsAsync();
            ListRangeTests.GetExistingListRangeByPassingJustListKeyAsync();
            ListRangeTests.GetExistingListRangeByPassingValidArgumentsAsync();
            ListRangeTests.GetNonExistingListRangeByPassingJustListKeyAsync();
        }

        private static void RunListLengthTests()
        {
            ListLengthTests.GetLentghOfExistingList();
            ListLengthTests.GetLentghOfExistingListAsync();
            ListLengthTests.GetLentghOfNonExistingList();
            ListLengthTests.GetLentghOfNonExistingListAsync();
        }

        private static void RunBulkLeftPushTests()
        {
            BulkListLeftPushTests.LeftPushBulkItemsInExistingList();
            BulkListLeftPushTests.LeftPushBulkItemsInNonExistingList();
            BulkListLeftPushTests.LeftPushItemInExistingListAsync();
            BulkListLeftPushTests.LeftPushItemInNonExistingListAsync();
        }

        private static void RunListLeftPushTests()
        {
            ListLeftPushTests.LeftPushItemInExistingList();
            ListLeftPushTests.LeftPushItemInExistingListAsync();
            ListLeftPushTests.LeftPushItemInNonExistingList();
            ListLeftPushTests.LeftPushItemInNonExistingListAsync();
        }

        private static void RunListInsertBeforeTests()
        {
            ListInsertBeforeTests.InsertBeforeExistingPivotAndList();
            ListInsertBeforeTests.InsertBeforeExistingPivotAndListAsync();
            ListInsertBeforeTests.InsertBeforeNonExistingPivotAndExistingList();
            ListInsertBeforeTests.InsertBeforeNonExistingPivotAndExistingListAsync();
            ListInsertBeforeTests.InsertBeforeNonExistingPivotAndList();
            ListInsertBeforeTests.InsertBeforeNonExistingPivotAndListAsync();
        }

        private static void RunListLeftPopTests()
        {
            ListLeftPopTests.LeftPopExistingItemFromExistingList();
            ListLeftPopTests.LeftPopNonExistingItemFromExistingList();
            ListLeftPopTests.LeftPopItemFromNonExistingList();
            ListLeftPopTests.LeftPopExistingItemFromExistingListAsync();
            ListLeftPopTests.LeftPopItemFromNonExistingListAsync();
            ListLeftPopTests.LeftPopNonExistingItemFromExistingListAsync();
        }
    }
}
