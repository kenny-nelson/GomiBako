#import <Foundation/Foundation.h>

/* 入出力するJSONファイル
 {
    "persons" : [
        { 
            "name" : "hoge",
            "age" : 2
        },
        {
            "name" : "fuga",
            "age" : 30
        },
        {
            "name" : "piyo",
            "age" : 400
        }
    ],
 
    "spots" : [
        {
            "name" : "Tokyo"
        },
        {
            "name" : "Kyoto"
        }
    ]
 }
 */

int main(int argc, const char * argv[]) {
    @autoreleasepool {
        NSString* fileName = @"json.dat";

        NSDictionary* person1 = @{ @"name" : @"hoge", @"age" : @2 };
        NSDictionary* person2 = @{ @"name" : @"fuga", @"age" : @30 };
        NSDictionary* person3 = @{ @"name" : @"piyo", @"age" : @400 };

        NSArray* persons = @[person1, person2, person3];

        NSDictionary* spot1 = @{ @"name" : @"Tokyo" };
        NSDictionary* spot2 = @{ @"name" : @"Kyoto" };

        NSArray* spots = @[spot1, spot2];

        NSDictionary* jsonDictionary = @{ @"persons" : persons, @"spots" : spots };

        NSOutputStream* outputStream = [[NSOutputStream alloc] initToFileAtPath:fileName append:NO];

        NSError* anyError;
        [outputStream open];
        [NSJSONSerialization writeJSONObject:jsonDictionary toStream:outputStream options:0 error:&anyError];
        [outputStream close];

        NSInputStream* inputStream = [[NSInputStream alloc] initWithFileAtPath:fileName];

        [inputStream open];
        NSDictionary* readJsonDictionary = [NSJSONSerialization JSONObjectWithStream:inputStream options:0 error:&anyError];
        [inputStream close];

        NSLog(@"%@", readJsonDictionary);
    }
    return 0;
}
