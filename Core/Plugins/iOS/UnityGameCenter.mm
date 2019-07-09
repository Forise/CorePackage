#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "UnityGameCenter.h"

// #define CLASS_NETWORK_DATA_MANAGER @"NetworkDataManager"
// #define HANDLE_RECEIVE_DATA  @"UnityReceiveData"
#define USER_DATA_CONTROL @"UserDataControl"

#ifdef __cplusplus
extern "C"
{
#endif
    typedef void (__stdcall * AuthenticateSuccess_Handler)(const char* userID,
                                                           const char* name, const char* underage, const char* isAuthenticated, const char* userAlias);
    typedef void (__stdcall * AuthenticateFailed_Handler)(const char* authenticateFailedCallback);
    typedef void (__stdcall * AvatarDownloaded_Handler)(const NSData* avatarDownloadedCallback);
    typedef void (__stdcall * GetMyUserID_Handler)(const char* userID,
                                                   const char* name, const char* underage, const char* isAuthenticated, const char* userAlias);
    typedef void (__stdcall * DataSaved_Handler)(const char* callback);
    typedef void (__stdcall * SaveDataFailed_Handler)(const char* error);
    typedef void (__stdcall * DataLoaded_Handler)(const char* data);
    typedef void (__stdcall * LoadDataFailed_Handler)(const char* error);
    typedef void (__stdcall * CheckSigningIn_Handler)(const char* isSignedIn);
    
    void UnitySendMessage(const char *className, const char *methodName, const char *param);
    
    GKLocalPlayer *localPlayer;
#ifdef __cplusplus
}
#endif

@implementation UnityGameCenter

+ (instancetype) sharedGameCenter
{
    static id instance = nil;
    
    @synchronized(self) {
        if (instance == nil)
            instance = [[self alloc] init];
    }
    
    return instance;
}

- (NSData*)convertCharToData:(const char*)ch
{
    NSString *s = [NSString stringWithCString:ch encoding:NSUTF8StringEncoding];
    NSData *d = [s dataUsingEncoding:NSUTF8StringEncoding];
    return d;
}

- (const char*)convertDataToChar:(NSData*)data
{
    NSString* aStr= [[NSString alloc] initWithData:data encoding:NSASCIIStringEncoding];
    const char* c = [aStr UTF8String];
    return c;
}

- (NSString*)convertDataToString:(NSData*)data
{
    NSString* aStr= [[NSString alloc] initWithData:data encoding:NSASCIIStringEncoding];
    return aStr;
}

@end

extern "C"
{
    void SendMessage(const char* targetClass, const char* metodName, const char* Params = nullptr)
    {
        if(!targetClass)
            return;
        UnitySendMessage(targetClass, metodName, Params != nullptr ? Params : nullptr);
    }
    
    void GetAvatar(AvatarDownloaded_Handler avatarDownloadedCallback)
    {
        [localPlayer loadPhotoForSize:GKPhotoSizeSmall withCompletionHandler:(^(UIImage *photo, NSError *error)
                                                                              {
                                                                                  UIImage *myUIImage = [UIImage imageNamed:@"Avatar.jpg"];
                                                                                  NSData *imageData = UIImagePNGRepresentation(photo);
                                                                                  NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
                                                                                  NSString *documentsDirectory = [paths objectAtIndex:0];
                                                                                  NSString *filePath = [documentsDirectory stringByAppendingPathComponent:@"Avatar.jpg"];
                                                                                  [imageData writeToFile:filePath atomically:YES];
                                                                                  printf_console("%s", [filePath UTF8String]);
                                                                                  avatarDownloadedCallback(imageData);
                                                                              })];
    }
    
    void AuthenticateLocalPlayer(AuthenticateSuccess_Handler authenticateSuccessCallback, AuthenticateFailed_Handler authenticateFailedCallback)
    {
        localPlayer = [GKLocalPlayer localPlayer];
        [localPlayer setAuthenticateHandler:(^(UIViewController* viewСontroller, NSError* error)
                                             {
                                                 if (viewСontroller != nil)
                                                 {
                                                     [UnityGetGLViewController() presentViewController:viewСontroller animated:YES completion:nil];
                                                 }
                                                 else if (localPlayer.isAuthenticated)
                                                 {
                                                     authenticateSuccessCallback([localPlayer.playerID UTF8String], [localPlayer.displayName UTF8String],
                                                                                 localPlayer.underage ? "true" : "false", localPlayer.isAuthenticated ? "true" : "false", [localPlayer.alias UTF8String]);
                                                 }
                                                 else
                                                 {
                                                     authenticateFailedCallback("NATIVE AUTENTICATE LOCAL PLAYER HAS ERROR!!!");
                                                 }
                                             })];
    }
    
    void CheckSigningIn(CheckSigningIn_Handler checkSigningIn_callback)
    {
        NSLog(@"Native: Checking IsSignedIn");
        checkSigningIn_callback(localPlayer.isAuthenticated ? "true" : "false");
        NSLog(@"Native: Signed In Callbak send");
    }
    
    void SaveUserDataToGameCenter(DataSaved_Handler dataSaved_callback, SaveDataFailed_Handler saveDataFailed_callback, const char* json)
    {
        printf("Native: Start saving data from native");
        printf("%s", json);
        NSData *data = [[UnityGameCenter sharedGameCenter] convertCharToData:json];
        printf("Native: NSData created");
        [localPlayer saveGameData:data withName:localPlayer.playerID completionHandler:(^(GKSavedGame * _Nullable savedGame, NSError * _Nullable error)
                                                                                        {
                                                                                            NSLog(@"Native: Local player save data");
                                                                                            if(!error)
                                                                                            {
                                                                                                NSLog(@"Native: Data saved");
                                                                                                dataSaved_callback("Data saved success!");
                                                                                                NSLog(@"Native: Data saved callback");
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                NSLog(@"Native: Error on saving data");
                                                                                                saveDataFailed_callback([error.localizedDescription UTF8String]);
                                                                                                NSLog(@"Native: Error callback send");
                                                                                            }
                                                                                        })];
    }
    
    void LoadUserData(DataLoaded_Handler dataLoaded_callback, LoadDataFailed_Handler loadDataFailed_callback)
    {
        if(localPlayer.authenticated)
        {
            [localPlayer fetchSavedGamesWithCompletionHandler:^(NSArray<GKSavedGame *> * _Nullable savedGames, NSError * _Nullable error)
             {
                 if(!error)
                 {
                     for (int i = 0; i < savedGames.count; i++)
                     {
                         if([savedGames[i].name  isEqual: localPlayer.playerID])
                         {
                             NSLog(@"UserData was found");
                             GKSavedGame *savedGame = savedGames[i];
                             [savedGame loadDataWithCompletionHandler:^(NSData * _Nullable data, NSError * _Nullable error)
                              {
                                  if(!error)
                                  {
                                      const char* json = [[UnityGameCenter sharedGameCenter] convertDataToChar:data];
                                      dataLoaded_callback(json);
                                      NSLog(@"Native: DATA LOADED FROM GC");
                                      //                                    [localPlayer deleteSavedGamesWithName:@"UserData" completionHandler:^(NSError * _Nullable error)
                                      //                                     {
                                      //                                         if(error)
                                      //                                         {
                                      //                                             NSLog(@"%@", error.localizedDescription);
                                      //                                         }
                                      //                                         else
                                      //                                             NSLog(@"UserData was deleted");
                                      //                                     }];
                                  }
                                  else
                                  {
                                      loadDataFailed_callback([error.localizedDescription UTF8String]);
                                      NSLog(@"Error on data loading");
                                      NSLog(@"%@", error.localizedDescription);
                                  }
                              }];
                             break;
                         }
                         NSLog(@"User data wasn't found");
                     }
                 }
                 else
                 {
                     loadDataFailed_callback([error.localizedDescription UTF8String]);
                     NSLog(@"Error on fecthing data");
                     NSLog(@"%@", error.localizedDescription);
                 }
             }];
        }
        else
        {
            loadDataFailed_callback("Local player is not authenticated");
            NSLog(@"Local player is not authenticated");
        }
    }
    
    void GetMyUserID(GetMyUserID_Handler getMyUserID_Callback)
    {
        getMyUserID_Callback([localPlayer.playerID UTF8String], [localPlayer.displayName UTF8String],
                             localPlayer.underage ? "true" : "false", localPlayer.isAuthenticated ? "true" : "false", [localPlayer.alias UTF8String]);
    }
}
