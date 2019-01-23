//
//  BuglyAgent.h
//  UnityCommonBridges
//
//  Created by leon on 2019/1/10.
//  Copyright © 2019 CDLX. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <Bugly/Bugly.h>

NS_ASSUME_NONNULL_BEGIN

@interface BuglyAgent : NSObject <BuglyDelegate>

+ (instancetype)shared;

@end

NS_ASSUME_NONNULL_END
