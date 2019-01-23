//
//  CommunicateUtil.m
//  UnityCommonBridges
//
//  Created by leon on 2019/1/10.
//  Copyright © 2019 CDLX. All rights reserved.
//

#import "CommunicateUtil.h"

extern void UnitySendMessage(const char *, const char *, const char *);


@implementation CommunicateUtil

+ (void)callCSharpMethod:(NSString*)methodName toGameObject:(NSString *)gameObject withParameters:(NSString *)parameters {
    if (!methodName || !gameObject) return;
    
    if(gameObject) {
        UnitySendMessage([gameObject cStringUsingEncoding: NSStringEncodingConversionAllowLossy],
                         [methodName cStringUsingEncoding: NSStringEncodingConversionAllowLossy],
                         parameters ? [parameters cStringUsingEncoding: NSStringEncodingConversionAllowLossy] : NULL);
        NSLog(@"callCSharpMethod: %@ %@ %@", gameObject, methodName, parameters);
    }
}

@end
