using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SettingPara.Wdata
{
    public class WOrderInfo
    {
        //PLC写入指令
        public string Write()
        {
            try
            {
                int list = 0;
                int blank = 0;
                int floor = 0;
                string str = null;
                var db = new DBAccess_MySql("MySql");
                string outString = "";
                DataTable dt;
                string sql;
                sql = "SELECT order_user,order_id,order_style FROM order_info WHERE order_state = 0 LIMIT 1";
                var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
                DataRow dr = dt.Rows[0];
                string order_user = dr[0].ToString();
                string orderid = dr[1].ToString();
                string orderstyle = dr[2].ToString();
                if (orderstyle == "1")
                {
                    str = "入库";
                }
                if (orderstyle == "2")
                {
                    str = "出库";
                }

                #region 入库
                if (str == "入库")
                {
                    //指令过账
                    sql = "SELECT goods_code,goods_name,goods_style FROM goods_info WHERE goods_id = " + int.Parse(order_user) + "";
                    var rlt1 = db.QuerySQL_ToTable(sql, out dt, out outString);
                    DataRow dr1 = dt.Rows[0];
                    string goods_code = dr1[0].ToString();
                    string goods_name = dr1[1].ToString();
                    string goods_style = dr1[2].ToString();

                    sql = "SELECT house_number,line,list,blank,floor FROM house_info WHERE house_state = 'I' and house_number = '" + goods_style + "'";
                    db.QuerySQL_ToTable(sql, out dt, out outString);
                    if (dt == null || dt.Rows.Count <= 0) return "S";
                    list = int.Parse(dt.Rows[0]["list"].ToString());
                    blank = int.Parse(dt.Rows[0]["blank"].ToString());
                    floor = int.Parse(dt.Rows[0]["floor"].ToString());

                    if (orderstyle == "1")//下达入库
                    {
                        //////跟新货位状态
                        //sql = "UPDATE house_info SET house_state = 'S' WHERE house_number = '" + goods_style + "'";
                        //db.ExecSql(sql, out outString);

                        ////删除
                        //sql = "UPDATE order_info SET order_state = 3 WHERE order_id = " + int.Parse(orderid) + "";
                        //db.ExecSql(sql, out outString);
                        foreach (var s in PLCSystem_SRM.Dic_SRM_Request)
                        {
                            s.Value.s_serial = 1;
                            s.Value.s_pattern = 2;

                            if (goods_name == "端拾器")
                            {
                                s.Value.s_FrStand = 1;
                            }

                            if (goods_name == "检具")
                            {
                                s.Value.s_FrStand = 2;
                            }

                            s.Value.s_FrLine = 0;
                            s.Value.s_FrGrid = 0;
                            s.Value.s_FrTier = 0;

                            s.Value.s_ToStand = 0;
                            s.Value.s_ToLine = (ushort)list;
                            s.Value.s_ToGrid = (ushort)blank;
                            s.Value.s_ToTier = (ushort)floor;

                            //s.Value.s_execute_signal = 0;

                            break;
                        }
                    }
                    if (orderstyle == "2")//下达出库
                    {
                        ////删除物料库存
                        //////跟新货位状态
                        //sql = "UPDATE house_info SET house_state = 'N' WHERE house_number = '" + goods_style + "'";
                        //db.ExecSql(sql, out outString);

                        ////删除
                        //sql = "DELETE FROM house_data WHERE house_number = '" + goods_style + "'";
                        //db.ExecSql(sql, out outString);

                        //sql = "UPDATE order_info SET order_state = 3 WHERE order_id = " + int.Parse(orderid) + "";
                        //db.ExecSql(sql, out outString);

                        foreach (var s in PLCSystem_SRM.Dic_SRM_Request)
                        {
                            s.Value.s_serial = 1;
                            s.Value.s_pattern = 1;

                            s.Value.s_FrStand = 0;
                            s.Value.s_FrLine = (ushort)list;
                            s.Value.s_FrGrid = (ushort)blank;
                            s.Value.s_FrTier = (ushort)floor;

                            if (goods_name == "端拾器")
                            {
                                s.Value.s_ToStand = 1;
                            }

                            if (goods_name == "检具")
                            {
                                s.Value.s_ToStand = 2;
                            }

                            s.Value.s_ToLine = 0;
                            s.Value.s_ToGrid = 0;
                            s.Value.s_ToTier = 0;

                            //s.Value.s_execute_signal = 0;

                            break;
                        }

                    }
                }
                #endregion

                #region 出库
                if (str == "出库")
                {
                    //指令过账
                    sql = "SELECT goods_code,goods_name,goods_style FROM goods_info WHERE goods_id = " + int.Parse(order_user) + "";
                    var rlt1 = db.QuerySQL_ToTable(sql, out dt, out outString);
                    DataRow dr1 = dt.Rows[0];
                    string goods_code = dr1[0].ToString();
                    string goods_name = dr1[1].ToString();
                    string goods_style = dr1[2].ToString();

                    sql = "SELECT house_number,line,list,blank,floor FROM house_info WHERE house_state = 'O' and house_number = '" + goods_style + "'";
                    db.QuerySQL_ToTable(sql, out dt, out outString);
                    if (dt == null || dt.Rows.Count <= 0) return "S";
                    list = int.Parse(dt.Rows[0]["list"].ToString());
                    blank = int.Parse(dt.Rows[0]["blank"].ToString());
                    floor = int.Parse(dt.Rows[0]["floor"].ToString());

                    if (orderstyle == "1")//写入 入库指令
                    {
                        //////跟新货位状态
                        //sql = "UPDATE house_info SET house_state = 'S' WHERE house_number = '" + goods_style + "'";
                        //db.ExecSql(sql, out outString);

                        ////删除
                        //sql = "UPDATE order_info SET order_state = 3 WHERE order_id = " + int.Parse(orderid) + "";
                        //db.ExecSql(sql, out outString);

                        foreach (var s in PLCSystem_SRM.Dic_SRM_Request)
                        {
                            s.Value.s_serial = 1;
                            s.Value.s_pattern = 2;

                            if (goods_name == "端拾器")
                            {
                                s.Value.s_FrStand = 1;
                            }

                            if (goods_name == "检具")
                            {
                                s.Value.s_FrStand = 2;
                            }

                            s.Value.s_FrLine = 0;
                            s.Value.s_FrGrid = 0;
                            s.Value.s_FrTier = 0;

                            s.Value.s_ToStand = 0;
                            s.Value.s_ToLine = (ushort)list;
                            s.Value.s_ToGrid = (ushort)blank;
                            s.Value.s_ToTier = (ushort)floor;

                            //s.Value.s_execute_signal = 0;

                            break;
                        }

                    }
                    if (orderstyle == "2")//写入 出库指令
                    {
                        ////删除物料库存
                        //////跟新货位状态
                        //sql = "UPDATE house_info SET house_state = 'N' WHERE house_number = '" + goods_style + "'";
                        //db.ExecSql(sql, out outString);

                        ////删除
                        //sql = "DELETE FROM house_data WHERE house_number = '" + goods_style + "'";
                        //db.ExecSql(sql, out outString);

                        //sql = "UPDATE order_info SET order_state = 3 WHERE order_id = " + int.Parse(orderid) + "";
                        //db.ExecSql(sql, out outString);

                        foreach (var s in PLCSystem_SRM.Dic_SRM_Request)
                        {
                            s.Value.s_serial = 1;
                            s.Value.s_pattern = 1;

                            s.Value.s_FrStand = 0;
                            s.Value.s_FrLine = (ushort)list;
                            s.Value.s_FrGrid = (ushort)blank;
                            s.Value.s_FrTier = (ushort)floor;

                            if (goods_name == "端拾器")
                            {
                                s.Value.s_ToStand = 1;
                            }

                            if (goods_name == "检具")
                            {
                                s.Value.s_ToStand = 2;
                            }

                            s.Value.s_ToLine = 0;
                            s.Value.s_ToGrid = 0;
                            s.Value.s_ToTier = 0;

                            //s.Value.s_execute_signal = 0;

                            break;
                        }

                    }
                }
                #endregion

                return "S";
            }
            catch
            {
                return "N";
            }
        }

        //系统过账
        public void Account()
        {
            string str = null;
            var db = new DBAccess_MySql("MySql");
            string outString = "";
            DataTable dt;
            string sql;
            sql = "SELECT order_user,order_id,order_style FROM order_info WHERE order_state = 0 LIMIT 1";
            var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
            DataRow dr = dt.Rows[0];
            string order_user = dr[0].ToString();
            string orderid = dr[1].ToString();
            string orderstyle = dr[2].ToString();
            if (orderstyle == "1")
            {
                str = "入库";
            }
            if (orderstyle == "2")
            {
                str = "出库";
            }

            #region 入库过账
            if (str == "入库")
            {
                sql = "SELECT goods_code,goods_name,goods_style FROM goods_info WHERE goods_id = " + int.Parse(order_user) + "";
                var rlt1 = db.QuerySQL_ToTable(sql, out dt, out outString);
                DataRow dr1 = dt.Rows[0];
                string goods_code = dr1[0].ToString();
                string goods_name = dr1[1].ToString();
                string goods_style = dr1[2].ToString();

                if (orderstyle == "1")//入库
                {
                    ////跟新货位状态
                    //sql = "UPDATE house_info SET house_state = 'S' WHERE house_number = '" + goods_style + "'";
                    //db.ExecSql(sql, out outString);
                    //修改指令
                    sql = "UPDATE order_info SET order_state = 1 WHERE order_id = " + int.Parse(orderid) + "";
                    db.ExecSql(sql, out outString);
                    ////插入数据到库存表
                    //sql = "INSERT INTO house_data(house_number,goods_code,goods_name,have_number,in_house_time) VALUES('" + goods_style + "','" + goods_name + "','" + goods_code + "'," + 1 + ",SYSDATE())";
                    //db.ExecSql(sql, out outString);
                }
                if (orderstyle == "2")//出库
                {
                    ////跟新货位状态
                    //sql = "UPDATE house_info SET house_state = 'N' WHERE house_number = '" + goods_style + "'";
                    //db.ExecSql(sql, out outString);

                    ////删除库存表数据
                    //sql = "DELETE FROM house_data WHERE house_number = '" + goods_style + "'";
                    //db.ExecSql(sql, out outString);

                    //修改指令状态
                    sql = "UPDATE order_info SET order_state = 1 WHERE order_id = " + int.Parse(orderid) + "";
                    db.ExecSql(sql, out outString);
                }
            }
            #endregion

            #region 出库过账
            if (str == "出库")
            {
                sql = "SELECT goods_code,goods_name,goods_style FROM goods_info WHERE goods_id = " + int.Parse(order_user) + "";
                var rlt1 = db.QuerySQL_ToTable(sql, out dt, out outString);
                DataRow dr1 = dt.Rows[0];
                string goods_code = dr1[0].ToString();
                string goods_name = dr1[1].ToString();
                string goods_style = dr1[2].ToString();

                if (orderstyle == "1")//入库
                {
                    ////跟新货位状态
                    //sql = "UPDATE house_info SET house_state = 'S' WHERE house_number = '" + goods_style + "'";
                    //db.ExecSql(sql, out outString);
                    //更新排队指令
                    sql = "UPDATE order_info SET order_state = 1 WHERE order_id = " + int.Parse(orderid) + "";
                    db.ExecSql(sql, out outString);
                }
                if (orderstyle == "2")//出库
                {
                    ////跟新货位状态
                    //sql = "UPDATE house_info SET house_state = 'N' WHERE house_number = '" + goods_style + "'";
                    //db.ExecSql(sql, out outString);

                    ////删除
                    //sql = "DELETE FROM house_data WHERE house_number = '" + goods_style + "'";
                    //db.ExecSql(sql, out outString);

                    sql = "UPDATE order_info SET order_state = 1 WHERE order_id = " + int.Parse(orderid) + "";
                    db.ExecSql(sql, out outString);
                }
            }
            #endregion
        }

        //清除过账
        public void ClearAccount()
        {
            try
            {
                string str = null;
                var db = new DBAccess_MySql("MySql");
                string outString = "";
                DataTable dt;
                string sql;
                sql = "SELECT order_user,order_id,order_style FROM order_info WHERE order_state = 1 LIMIT 1";
                var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
                DataRow dr = dt.Rows[0];
                string order_user = dr[0].ToString();
                string orderid = dr[1].ToString();
                string orderstyle = dr[2].ToString();
                if (orderstyle == "1")
                {
                    str = "入库";
                }
                if (orderstyle == "2")
                {
                    str = "出库";
                }

                #region 入库过账
                if (str == "入库")
                {
                    sql = "SELECT goods_code,goods_name,goods_style FROM goods_info WHERE goods_id = " + int.Parse(order_user) + "";
                    var rlt1 = db.QuerySQL_ToTable(sql, out dt, out outString);
                    DataRow dr1 = dt.Rows[0];
                    string goods_code = dr1[0].ToString();
                    string goods_name = dr1[1].ToString();
                    string goods_style = dr1[2].ToString();

                    if (orderstyle == "1")//入库
                    {
                        //跟新货位状态
                        sql = "UPDATE house_info SET house_state = 'S' WHERE house_number = '" + goods_style + "'";
                        db.ExecSql(sql, out outString);
                        //修改指令
                        sql = "UPDATE order_info SET order_state = 3 WHERE order_id = " + int.Parse(orderid) + "";
                        db.ExecSql(sql, out outString);
                        //插入数据到库存表
                        sql = "INSERT INTO house_data(house_number,goods_code,goods_name,have_number,in_house_time) VALUES('" + goods_style + "','" + goods_name + "','" + goods_code + "'," + 1 + ",SYSDATE())";
                        db.ExecSql(sql, out outString);
                    }
                    if (orderstyle == "2")//出库
                    {
                        //跟新货位状态
                        sql = "UPDATE house_info SET house_state = 'N' WHERE house_number = '" + goods_style + "'";
                        db.ExecSql(sql, out outString);

                        //删除库存表数据
                        sql = "DELETE FROM house_data WHERE house_number = '" + goods_style + "'";
                        db.ExecSql(sql, out outString);

                        //修改指令状态
                        sql = "UPDATE order_info SET order_state = 3 WHERE order_id = " + int.Parse(orderid) + "";
                        db.ExecSql(sql, out outString);
                    }
                }
                #endregion

                #region 出库过账
                if (str == "出库")
                {
                    sql = "SELECT goods_code,goods_name,goods_style FROM goods_info WHERE goods_id = " + int.Parse(order_user) + "";
                    var rlt1 = db.QuerySQL_ToTable(sql, out dt, out outString);
                    DataRow dr1 = dt.Rows[0];
                    string goods_code = dr1[0].ToString();
                    string goods_name = dr1[1].ToString();
                    string goods_style = dr1[2].ToString();

                    if (orderstyle == "1")//入库
                    {
                        //跟新货位状态
                        sql = "UPDATE house_info SET house_state = 'S' WHERE house_number = '" + goods_style + "'";
                        db.ExecSql(sql, out outString);
                        //更新排队指令
                        sql = "UPDATE order_info SET order_state = 3 WHERE order_id = " + int.Parse(orderid) + "";
                        db.ExecSql(sql, out outString);
                    }
                    if (orderstyle == "2")//出库
                    {
                        //跟新货位状态
                        sql = "UPDATE house_info SET house_state = 'N' WHERE house_number = '" + goods_style + "'";
                        db.ExecSql(sql, out outString);

                        //删除
                        sql = "DELETE FROM house_data WHERE house_number = '" + goods_style + "'";
                        db.ExecSql(sql, out outString);

                        sql = "UPDATE order_info SET order_state = 3 WHERE order_id = " + int.Parse(orderid) + "";
                        db.ExecSql(sql, out outString);
                    }
                }
                #endregion
            }
            catch
            {

            }
        }

        //故障过账
        public void Fault()
        {
            #region 指令过账
            var db = new DBAccess_MySql("MySql");
            string outString = "";
            DataTable dt;
            string sql;
            sql = "SELECT order_user,order_id,order_style FROM order_info WHERE order_state = 1 LIMIT 1";
            var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
            DataRow dr = dt.Rows[0];
            string order_user = dr[0].ToString();
            string orderid = dr[1].ToString();
            string orderstyle = dr[2].ToString();

            sql = "SELECT goods_code,goods_name,goods_style FROM goods_info WHERE goods_id = " + int.Parse(order_user) + "";
            var rlt1 = db.QuerySQL_ToTable(sql, out dt, out outString);
            DataRow dr1 = dt.Rows[0];
            string goods_code = dr1[0].ToString();
            string goods_name = dr1[1].ToString();
            string goods_style = dr1[2].ToString();

            if (orderstyle == "1")//入库
            {
                ////跟新货位状态
                sql = "UPDATE house_info SET house_state = 'N' WHERE house_number = '" + goods_style + "'";
                db.ExecSql(sql, out outString);

                sql = "UPDATE order_info SET order_state = 2 WHERE order_id = " + int.Parse(orderid) + "";
                db.ExecSql(sql, out outString);

            }
            if (orderstyle == "2")//出库
            {
                ////跟新货位状态
                sql = "UPDATE house_info SET house_state = 'S' WHERE house_number = '" + goods_style + "'";
                db.ExecSql(sql, out outString);

                //更新库存数据
                sql = "INSERT INTO house_data(house_number,goods_code,goods_name,have_number,in_house_time) VALUES('" + goods_style + "','" + goods_name + "','" + goods_code + "'," + 1 + ",SYSDATE())";
                db.ExecSql(sql, out outString);

                sql = "UPDATE order_info SET order_state = 2 WHERE order_id = " + int.Parse(orderid) + "";
                db.ExecSql(sql, out outString);
            }
            #endregion
        }

        //判断当前指令0
        public int OrderInfo()
        {
            try
            {
                var db = new DBAccess_MySql("MySql");
                string outString = "";
                DataTable dt;
                string sql;
                sql = "SELECT order_user,order_id,order_style,order_state FROM order_info WHERE order_state = 0 LIMIT 1";
                var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
                DataRow dr = dt.Rows[0];
                string order_user = dr[0].ToString();
                string orderid = dr[1].ToString();
                string orderstyle = dr[2].ToString();
                string order_state = dr[3].ToString();

                return int.Parse(order_state);
            }
            catch
            {
                return 1;
            }
        }

        //判断当前指令
        public string OrderInfo(int s_pattern)
        {
            if (s_pattern == 1)//出库
            {
                return "出库";
            }
            else if (s_pattern == 2)//入库
            {
                return "入库";
            }
            else
            {
                return "无";
            }
        }

        //出库过账
        public void OutAccount(int s_FrLine,int s_FrGrids,int s_FrTiers,int s_ToStand)
        {
            string houseNumber = "0" + s_FrLine + "0" + s_FrGrids + "0" + s_FrTiers;

            var db = new DBAccess_MySql("MySql");
            string outString = "";
            DataTable dt;
            string sql;
            sql = "SELECT goods_id,goods_code,goods_name FROM goods_info WHERE goods_style = '"+houseNumber+ "'";
            var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
            DataRow dr = dt.Rows[0];
            string goods_id = dr[0].ToString();
            string goods_code = dr[1].ToString();
            string goods_name = dr[2].ToString();

            //跟新货位状态
            sql = "UPDATE house_info SET house_state = 'N' WHERE house_number = '" + houseNumber + "'";
            db.ExecSql(sql, out outString);

            //删除库存表数据
            sql = "DELETE FROM house_data WHERE house_number = '" + houseNumber + "'";
            db.ExecSql(sql, out outString);

            sql = "UPDATE order_info SET order_state = 3 WHERE order_state = 1 LIMIT 1";
            db.ExecSql(sql, out outString);
        }
            
        //入库过账
        public void InAccount(int s_FrStand, int s_ToLine, int s_ToGrid, int s_ToTier)
        {
            string houseNumber = "0" + s_ToLine + "0" + s_ToGrid + "0" + s_ToTier;

            var db = new DBAccess_MySql("MySql");
            string outString = "";
            DataTable dt;
            string sql;
            sql = "SELECT goods_id,goods_code,goods_name FROM goods_info WHERE goods_style = '" + houseNumber + "'";
            var rlt = db.QuerySQL_ToTable(sql, out dt, out outString);
            DataRow dr = dt.Rows[0];
            string goods_id = dr[0].ToString();
            string goods_code = dr[1].ToString();
            string goods_name = dr[2].ToString();

            //跟新货位状态
            sql = "UPDATE house_info SET house_state = 'O' WHERE house_number = '" + houseNumber + "'";
            db.ExecSql(sql, out outString);

            //更新库存数据
            sql = "INSERT INTO house_data(house_number,goods_code,goods_name,have_number,in_house_time) VALUES('" + houseNumber + "','" + goods_name + "','" + goods_code + "'," + 1 + ",SYSDATE())";
            db.ExecSql(sql, out outString);

            sql = "UPDATE order_info SET order_state = 3 WHERE order_state = 1 LIMIT 1";
            db.ExecSql(sql, out outString);
        }
    }
}
