//
//  BuglyAttachCollector.m
//  UnityCommonBridges
//
//  Created by leon on 2019/1/10.
//  Copyright © 2019 CDLX. All rights reserved.
//

#import "BuglyAttachCollector.h"

static NSUInteger kMaxCollectionCount = 50;
static NSObject *attachMessageQueueLocker;

@interface BuglyAttachCollector()
{
    NSMutableArray  *_attachMessageQueue;
}

@end

@implementation BuglyAttachCollector

+ (instancetype)shared {
    static BuglyAttachCollector *collector;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        collector = [BuglyAttachCollector new];
    });
    
    return collector;
}

- (id)init {
    if (self = [super init]) {
        _attachMessageQueue = [NSMutableArray arrayWithCapacity:kMaxCollectionCount];
    }
    
    return self;
}

- (void)appendAttachMessage:(NSString *)message {
    if (!message) return;
    
    @synchronized (attachMessageQueueLocker) {
        if (_attachMessageQueue.count >= kMaxCollectionCount) {
            [_attachMessageQueue removeObjectAtIndex:0];
        }
        [_attachMessageQueue addObject:message];
    }
}

- (nullable NSString *)attachMessages {
    NSString *messages = nil;
    @synchronized (attachMessageQueueLocker) {
        messages = [_attachMessageQueue componentsJoinedByString:@"\n\n"];
    }
    
    return messages;
}

- (void)clean {
    @synchronized (attachMessageQueueLocker) {
        [_attachMessageQueue removeAllObjects];
    }
}

@end
