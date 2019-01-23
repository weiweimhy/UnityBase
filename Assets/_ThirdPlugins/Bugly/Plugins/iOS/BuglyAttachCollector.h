//
//  BuglyAttachCollector.h
//  UnityCommonBridges
//
//  Created by leon on 2019/1/10.
//  Copyright © 2019 CDLX. All rights reserved.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface BuglyAttachCollector : NSObject

+ (instancetype)shared;

- (void)appendAttachMessage:(NSString *)message;
- (nullable NSString *)attachMessages;
- (void)clean;

@end

NS_ASSUME_NONNULL_END
