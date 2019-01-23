//
//  JSONHelper.h
//  UnityCommonBridges
//
//  Created by leon on 2019/1/11.
//  Copyright © 2019 CDLX. All rights reserved.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface JSONHelper : NSObject

+ (nullable NSDictionary *)convertNString2Dictionary:(NSString *)json;
+ (nullable NSDictionary *)convertCString2Dictionary:(const char *)json;
+ (nullable NSString *)convertDictionary2NSString:(NSDictionary *)dictionary;
+ (nullable const char *)convertDictionary2CString:(NSDictionary *)dictionary;

@end

NS_ASSUME_NONNULL_END
