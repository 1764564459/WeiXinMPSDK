﻿#region Apache License Version 2.0
/*----------------------------------------------------------------

Copyright 2022 Jeffrey Su & Suzhou Senparc Network Technology Co.,Ltd.

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file
except in compliance with the License. You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the
License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
either express or implied. See the License for the specific language governing permissions
and limitations under the License.

Detail: https://github.com/JeffreySu/WeiXinMPSDK/blob/master/license.md

----------------------------------------------------------------*/
#endregion Apache License Version 2.0

/*----------------------------------------------------------------
    Copyright (C) 2022 Senparc
    
    文件名：RequestMessageEvent_UserEnterTempSession.cs
    文件功能描述：事件之用户进入客服
    
    
    创建标识：Senparc - 20170107
    
----------------------------------------------------------------*/

using Senparc.Weixin.WxOpen;
using Senparc.Weixin.WxOpen.Entities;

namespace Senparc.Weixin.WxOpen.Entities
{
    /// <summary>
    /// 事件之用户进入客服
    /// </summary>
    public class RequestMessageEvent_UserEnterTempSession : RequestMessageEventBase, IRequestMessageEventBase
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public override Event Event
        {
            get { return Event.user_enter_tempsession; }
        }

        /// <summary>
        /// 开发者在客服会话按钮设置的sessionFrom参数
        /// </summary>
        public string SessionFrom { get; set; }

    }
}
