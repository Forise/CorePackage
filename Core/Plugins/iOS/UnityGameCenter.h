extern NSString *const PresentAuthenticationViewController;
extern NSString *const LocalPlayerIsAuthenticated;

@protocol UnityGameCenterDelegate
@end

@interface UnityGameCenter : NSObject
@property (nonatomic, readonly) NSError *lastError;
@property (nonatomic, strong) NSMutableDictionary *playersDict;
@end
