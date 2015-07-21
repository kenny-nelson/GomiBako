#import <Foundation/Foundation.h>

@interface AppManager : NSObject
{
    int _count;
}

+ (instancetype)sharedManager;

@property (nonatomic, getter=getCount, setter=setCount:)int count;

@end

@implementation AppManager

+ (instancetype)sharedManager
{
    static AppManager* manager = nil;
    static dispatch_once_t once;

    dispatch_once(&once, ^{
        manager = [[self alloc] init];
    });

    return manager;
}

- (int)getCount
{
    return _count;
}

- (void)setCount:(int)count
{
    _count = count;
}

@end

int main(int argc, const char * argv[]) {
    @autoreleasepool {
        // シングルトンパターンで取得したインスタンス
        {
            AppManager* manager = [AppManager sharedManager];
            ++manager.count;
            NSLog(@"Count %d", manager.count);
        }

        // シングルトンパターンで取得したインスタンス
        {
            AppManager* manager = [AppManager sharedManager];
            ++manager.count;
            NSLog(@"Count %d", manager.count);
        }

        // 通常の初期化処理で取得したのでインスタンスが変わる。
        {
            AppManager* manager = [[AppManager alloc] init];
            ++manager.count;
            NSLog(@"Count %d", manager.count);
        }
    }
    return 0;
}
