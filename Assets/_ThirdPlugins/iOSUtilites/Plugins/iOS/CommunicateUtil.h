//
//  CommunicateUtil.h
//  UnityCommonBridges
//
//  Created by leon on 2019/1/10.
//  Copyright © 2019 CDLX. All rights reserved.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface CommunicateUtil : NSObject

+ (void)callCSharpMethod:(NSString*)methodName toGameObject:(NSString *)gameObject withParameters:(NSString *)parameters;

@end

NS_ASSUME_NONNULL_END
