using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


namespace View
{
    public class View_BaseDisplayInfo : MonoBehaviour
    {
        public Text TxtCurrentTime;                                             //系统当前时间


        private void Update()
        {
            //获取系统当前时间
            DateTime NowTime = DateTime.Now.ToLocalTime();
            TxtCurrentTime.text = NowTime.ToString("yyyy-MM-dd HH:mm:ss");

        }

    }//class_end
}
