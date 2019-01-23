//
//  Appsflyer_iOS.m
//  UnityCommonBridges
//
//  Created by leon on 2019/1/10.
//  Copyright © 2019 CDLX. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <AppsFlyerLib/AppsFlyerTracker.h>
#import "AppsFlyerTracker+UnitySupport.h"
#import "CommunicateUtil.h"
#import "JSONHelper.h"


extern "C" {
    char * cStringCopy(const char* cString) {
        if (cString == NULL) {
            return NULL;
        }
        char *res = (char *)malloc(strlen(cString) + 1);
        strcpy(res, cString);
        return res;
    }
    
    void af_init(const char * key, const char * appid, const char * delegateObjectName, bool debug) {
        NSString *nsKey = [NSString stringWithCString:key encoding:NSStringEncodingConversionAllowLossy];
        assert(nsKey.length > 0);
        NSString *nsAppid= [NSString stringWithCString:appid encoding:NSStringEncodingConversionAllowLossy];
        assert(nsAppid.length > 0);
        NSString *nsDelegateObjectName= [NSString stringWithCString:delegateObjectName encoding:NSStringEncodingConversionAllowLossy];
        assert(nsDelegateObjectName.length > 0);

        [AppsFlyerTracker sharedTracker].appsFlyerDevKey = nsKey;
        [AppsFlyerTracker sharedTracker].appleAppID = nsAppid;
        [AppsFlyerTracker sharedTracker].delegate = [AppsFlyerTracker sharedTracker];
        [AppsFlyerTracker sharedTracker].delegateObjectName = nsDelegateObjectName;
    }
    
    void af_trackAppLaunch() {
        [[AppsFlyerTracker sharedTracker] trackAppLaunch];
    }
    
    void af_logEvent(const char * eventName, const char * json) {
        NSString *nsEventName = [NSString stringWithCString:eventName encoding:NSStringEncodingConversionAllowLossy];
        assert(nsEventName.length > 0);
        NSDictionary *JSONValue = [JSONHelper convertCString2Dictionary:json];

        [[AppsFlyerTracker sharedTracker] trackEvent:nsEventName withValues:JSONValue];
    }
    
    const char * af_getUID() {
        NSString *uid = [[AppsFlyerTracker sharedTracker] getAppsFlyerUID];
        if (!uid) {
            return NULL;
        }
        
        return cStringCopy([uid UTF8String]);
    }
    
    void af_validateAndTrackInAppPurchase(const char * productId, const char * price, const char * currency, const char * tranactionId, const char * json) {
        NSString *nsProductId = [NSString stringWithCString:productId encoding:NSStringEncodingConversionAllowLossy];
        NSString *nsPrice = [NSString stringWithCString:price encoding:NSStringEncodingConversionAllowLossy];
        NSString *nsCurrency = [NSString stringWithCString:currency encoding:NSStringEncodingConversionAllowLossy];
        NSString *nsTranactionId = [NSString stringWithCString:tranactionId encoding:NSStringEncodingConversionAllowLossy];
        NSDictionary *JSONValue = [JSONHelper convertCString2Dictionary:json];
        NSString *deleateObjectName = [AppsFlyerTracker sharedTracker].delegateObjectName;
        [[AppsFlyerTracker sharedTracker] validateAndTrackInAppPurchase:nsProductId price:nsPrice currency:nsCurrency transactionId:nsTranactionId additionalParameters:JSONValue success:^(NSDictionary *response) {
            NSString *responseJson = [JSONHelper convertDictionary2NSString:response];
            [CommunicateUtil callCSharpMethod:@"validateAndTrackInAppPurchaseSuccess"
                                 toGameObject:deleateObjectName
                               withParameters:responseJson];
        } failure:^(NSError *error, id reponse) {
            NSString *errJson = [JSONHelper convertDictionary2NSString:error.userInfo];
            [CommunicateUtil callCSharpMethod:@"validateAndTrackInAppPurchaseFailure"
                                 toGameObject:deleateObjectName
                               withParameters:errJson ? errJson : @"error"];
        }];
    }
}
