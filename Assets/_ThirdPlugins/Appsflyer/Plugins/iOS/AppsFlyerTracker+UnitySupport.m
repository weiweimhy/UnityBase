//
//  AppsFlyerTracker+UnitySupport.m
//  UnityCommonBridges
//
//  Created by leon on 2019/1/10.
//  Copyright © 2019 CDLX. All rights reserved.
//

#import <objc/runtime.h>
#import "AppsFlyerTracker+UnitySupport.h"
#import "CommunicateUtil.h"
#import "JSONHelper.h"


@implementation AppsFlyerTracker (UnitySupport)
@dynamic delegateObjectName;

- (void)setDelegateObjectName:(NSString *)delegateObjectName {
    objc_setAssociatedObject(self, @selector(delegateObjectName), delegateObjectName, OBJC_ASSOCIATION_RETAIN_NONATOMIC);
}

- (NSString *)delegateObjectName {
    return objc_getAssociatedObject(self, @selector(delegateObjectName));
}

- (void) onConversionDataReceived:(NSDictionary*) installData {
    NSString *installDataJson = [JSONHelper convertDictionary2NSString:installData];
    [CommunicateUtil callCSharpMethod:@"onConversionDataReceived"
                         toGameObject:self.delegateObjectName
                       withParameters:installDataJson];
}

@end
