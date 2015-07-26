#import <Foundation/Foundation.h>

int main(int argc, const char * argv[]) {
    @autoreleasepool {
        NSString* fileName = @"sample.xml";

        {
            NSXMLElement* root = [NSXMLNode elementWithName:@"persons"];
            NSXMLDocument* xmlDoc = [[NSXMLDocument alloc] initWithRootElement:root];
            [xmlDoc setVersion:@"1.0"];
            [xmlDoc setCharacterEncoding:@"UTF-8"];

            [root addChild:[NSXMLNode commentWithStringValue:@"Person List"]];
            NSXMLElement* person1 = [NSXMLNode elementWithName:@"person"];
            [person1 addAttribute:[NSXMLNode attributeWithName:@"name" stringValue:@"hoge"]];
            [person1 addAttribute:[NSXMLNode attributeWithName:@"age" stringValue:@"10"]];

            NSXMLElement* person2 = [NSXMLNode elementWithName:@"person"];
            [person2 addAttribute:[NSXMLNode attributeWithName:@"name" stringValue:@"fuga"]];
            [person2 addAttribute:[NSXMLNode attributeWithName:@"age" stringValue:@"200"]];

            NSXMLElement* person3 = [NSXMLNode elementWithName:@"person"];
            [person3 addAttribute:[NSXMLNode attributeWithName:@"name" stringValue:@"piyo"]];
            [person3 addAttribute:[NSXMLNode attributeWithName:@"age" stringValue:@"3000"]];

            [root addChild:person1];
            [root addChild:person2];
            [root addChild:person3];

            NSData* xmlData = [xmlDoc XMLDataWithOptions:NSXMLNodePrettyPrint];
            [xmlData writeToFile:fileName atomically:YES];
        }

        {
            NSURL* fileUrl = [NSURL fileURLWithPath:fileName];
            NSError* error = nil;
            NSXMLDocument* xmlDoc =
                [[NSXMLDocument alloc] initWithContentsOfURL:fileUrl
                                                     options:(NSXMLNodePreserveWhitespace|NSXMLNodePreserveCDATA)
                                                       error:&error];

            NSString* xmlDisplay = [xmlDoc description];
            NSLog(@"%@", xmlDisplay);
        }
    }
    return 0;
}
