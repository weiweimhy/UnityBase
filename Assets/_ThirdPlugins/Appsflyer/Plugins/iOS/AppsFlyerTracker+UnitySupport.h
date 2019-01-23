//
//  AppsFlyerTracker+UnitySupport.h
//  UnityCommonBridges
//
//  Created by leon on 2019/1/10.
//  Copyright © 2019 CDLX. All rights reserved.
//

#import <AppsFlyerLib/AppsFlyerTracker.h>

NS_ASSUME_NONNULL_BEGIN

@interface AppsFlyerTracker (UnitySupport) <AppsFlyerTrackerDelegate>
@property (nonatomic, strong) NSString      *delegateObjectName;

@end

NS_ASSUME_NONNULL_END
