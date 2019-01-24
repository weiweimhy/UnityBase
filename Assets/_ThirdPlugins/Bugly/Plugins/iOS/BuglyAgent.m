//
//  BuglyAgent.m
//  UnityCommonBridges
//
//  Created by leon on 2019/1/10.
//  Copyright © 2019 CDLX. All rights reserved.
//

#import "BuglyAgent.h"
#import "BuglyAttachCollector.h"

@implementation BuglyAgent

+ (instancetype)shared {
    static BuglyAgent *agent;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        agent = [BuglyAgent new];
    });
    
    return agent;
}

- (NSString * BLY_NULLABLE)attachmentForException:(NSException * BLY_NULLABLE)exception {
    NSString *attachment = [[BuglyAttachCollector shared] attachMessages];
    [[BuglyAttachCollector shared] clean];
    return attachment;
}

@end
