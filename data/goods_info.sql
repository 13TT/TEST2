/*
Navicat MySQL Data Transfer

Source Server         : database_lbt
Source Server Version : 50561
Source Host           : 127.0.0.1:3306
Source Database       : db_cawc

Target Server Type    : MYSQL
Target Server Version : 50561
File Encoding         : 65001

Date: 2019-04-12 13:08:33
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for goods_info
-- ----------------------------
DROP TABLE IF EXISTS `goods_info`;
CREATE TABLE `goods_info` (
  `goods_id` int(11) NOT NULL AUTO_INCREMENT,
  `goods_code` varchar(255) DEFAULT NULL,
  `goods_name` varchar(255) DEFAULT '',
  `goods_height` varchar(255) DEFAULT '' COMMENT '规格',
  `goods_differ` varchar(255) DEFAULT '',
  `goods_style` varchar(255) DEFAULT '',
  `goods_unit` varchar(255) DEFAULT '',
  `house_style` char(1) DEFAULT 'A',
  `unit_weight` varchar(255) DEFAULT '',
  `modify_time` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`goods_id`)
) ENGINE=InnoDB AUTO_INCREMENT=201 DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of goods_info
-- ----------------------------
INSERT INTO `goods_info` VALUES ('101', 'A端拾器C202翼子板', '端拾器', '1700', '', '010101', '', 'A', '', '2019-04-10 16:42:11');
INSERT INTO `goods_info` VALUES ('102', '检具S111后门外板（左）', '检具', '1700', '', '010102', '', 'A', '', '2019-04-10 16:44:40');
INSERT INTO `goods_info` VALUES ('103', 'A端拾器S111侧围右', '端拾器', '1700', '', '010103', '', 'A', '', '2019-04-07 19:56:37');
INSERT INTO `goods_info` VALUES ('104', '检具S111后门外板（左）', '检具', '1300', '', '010104', '', 'A', '', '2019-04-07 20:34:36');
INSERT INTO `goods_info` VALUES ('105', '检具C201前罩外板-14', '检具', '1300', '', '010105', '', 'A', '', '2019-04-07 20:34:50');
INSERT INTO `goods_info` VALUES ('106', '检具S111前地板左', '检具', '1300', '', '010106', '', 'A', '', '2019-04-07 20:35:34');
INSERT INTO `goods_info` VALUES ('107', '检具S101前罩内板-15', '检具', '1300', '', '010107', '', 'A', '', '2019-04-07 20:35:43');
INSERT INTO `goods_info` VALUES ('108', 'A端拾器C202侧围外蒙皮左', '端拾器', '1700', '', '010201', '', 'A', '', '2019-04-07 22:00:38');
INSERT INTO `goods_info` VALUES ('109', '检具C201前门总成右（老款）', '检具', '1700', '', '010202', '', 'A', '', '2019-04-07 22:01:12');
INSERT INTO `goods_info` VALUES ('110', 'A端拾器C212翼子板', '端拾器', '1700', '', '010203', '', 'A', '', '2019-04-07 22:01:36');
INSERT INTO `goods_info` VALUES ('111', '检具S111前门外板（左）', '检具', '1300', '', '010204', '', 'A', '', '2019-04-07 22:01:49');
INSERT INTO `goods_info` VALUES ('112', '检具S111前地板右', '检具', '1300', '', '010205', '', 'A', '', '2019-04-07 22:02:17');
INSERT INTO `goods_info` VALUES ('113', '检具S111前臂板', '检具', '1300', '', '010206', '', 'A', '', '2019-04-07 22:02:35');
INSERT INTO `goods_info` VALUES ('114', '检具C211后门内板左', '检具', '1300', '', '010207', '', 'A', '', '2019-04-07 22:02:53');
INSERT INTO `goods_info` VALUES ('115', 'A端拾器C211翼子板（左右）', '端拾器', '1700', '', '010301', '', 'A', '', '2019-04-07 22:03:36');
INSERT INTO `goods_info` VALUES ('116', '检具S101翼子板（左）', '检具', '1700', '', '010302', '', 'A', '', '2019-04-07 22:03:57');
INSERT INTO `goods_info` VALUES ('117', 'A线端拾器C202侧围右', '线端拾器', '1700', '', '010303', '', 'A', '', '2019-04-07 22:04:25');
INSERT INTO `goods_info` VALUES ('118', '检具S111后门外板（右）', '检具', '1300', '', '010304', '', 'A', '', '2019-04-07 22:04:48');
INSERT INTO `goods_info` VALUES ('119', '检具S101后门内板左-17', '检具', '1300', '', '010305', '', 'A', '', '2019-04-07 22:05:10');
INSERT INTO `goods_info` VALUES ('120', '检具S101前罩内板-17', '检具', '1300', '', '010306', '', 'A', '', '2019-04-07 22:05:32');
INSERT INTO `goods_info` VALUES ('121', '检具S101前罩外板-15', '检具', '1300', '', '010307', '', 'A', '', '2019-04-07 22:05:46');
INSERT INTO `goods_info` VALUES ('122', 'A端拾器C201侧围左', '端拾器', '1700', '', '010401', '', 'A', '', '2019-04-07 22:06:12');
INSERT INTO `goods_info` VALUES ('123', '检具C201后门总成左（老款）', '检具', '1700', '', '010402', '', 'A', '', '2019-04-07 22:06:31');
INSERT INTO `goods_info` VALUES ('124', 'B端拾器S101前门外板', '端拾器', '1700', '', '010403', '', 'A', '', '2019-04-07 22:07:03');
INSERT INTO `goods_info` VALUES ('125', '检具C211后门内板右', '检具', '1500', '', '010404', '', 'A', '', '2019-04-07 22:07:19');
INSERT INTO `goods_info` VALUES ('126', '检具S101前门外板右', '检具', '1300', '', '010405', '', 'A', '', '2019-04-07 22:07:37');
INSERT INTO `goods_info` VALUES ('127', '检具S101前罩总成（老款）', '检具', '1300', '', '010406', '', 'A', '', '2019-04-07 22:08:59');
INSERT INTO `goods_info` VALUES ('128', '检具C201前门内板右', '检具', '1300', '', '010407', '', 'A', '', '2019-04-07 22:09:15');
INSERT INTO `goods_info` VALUES ('129', 'B端拾器S101前罩外板-老款', '端拾器', '1700', '', '010501', '', 'A', '', '2019-04-07 22:09:35');
INSERT INTO `goods_info` VALUES ('130', '检具C201翼子板（左）', '检具', '1700', '', '010502', '', 'A', '', '2019-04-07 22:09:49');
INSERT INTO `goods_info` VALUES ('131', 'B端拾器C211侧围', '端拾器', '1700', '', '010503', '', 'A', '', '2019-04-07 22:10:15');
INSERT INTO `goods_info` VALUES ('132', 'A端拾器C201行李箱上块', '端拾器', '1500', '', '010504', '', 'A', '', '2019-04-07 22:10:36');
INSERT INTO `goods_info` VALUES ('133', 'A端拾器C201前门内板', '端拾器', '1300', '', '010505', '', 'A', '', '2019-04-07 22:11:02');
INSERT INTO `goods_info` VALUES ('134', 'B端拾器S111后地板', '端拾器', '1300', '', '010506', '', 'A', '', '2019-04-07 22:11:15');
INSERT INTO `goods_info` VALUES ('135', '检具C201后门内板右-16', '检具', '1300', '', '010507', '', 'A', '', '2019-04-07 22:12:26');
INSERT INTO `goods_info` VALUES ('136', 'A端拾器C211侧围左', '端拾器', '1700', '', '010601', '', 'A', '', '2019-04-07 22:12:34');
INSERT INTO `goods_info` VALUES ('137', '检具C201前门铰链', '检具', '1700', '', '010602', '', 'A', '', '2019-04-07 22:12:59');
INSERT INTO `goods_info` VALUES ('138', 'B端拾器S111前壁板', '端拾器', '1700', '', '010603', '', 'A', '', '2019-04-07 22:13:17');
INSERT INTO `goods_info` VALUES ('139', 'B端拾器S111前门外板', '端拾器', '1500', '', '010604', '', 'A', '', '2019-04-07 22:13:27');
INSERT INTO `goods_info` VALUES ('140', 'B端拾器C201前罩外板-U03', '端拾器', '1500', '', '010605', '', 'A', '', '2019-04-07 22:13:48');
INSERT INTO `goods_info` VALUES ('141', 'B端拾器S111前地板', '端拾器', '1300', '', '010606', '', 'A', '', '2019-04-07 22:14:00');
INSERT INTO `goods_info` VALUES ('142', '检具C201后门内板左', '检具', '1300', '', '010607', '', 'A', '', '2019-04-07 22:14:15');
INSERT INTO `goods_info` VALUES ('143', 'A线端拾器C212侧围左', '端拾器', '1700', '', '010701', '', 'A', '', '2019-04-07 22:14:46');
INSERT INTO `goods_info` VALUES ('144', '检具C201后门总成右（老款）', '检具', '1700', '', '010702', '', 'A', '', '2019-04-07 22:15:05');
INSERT INTO `goods_info` VALUES ('145', 'B端拾器S101前罩内板17款', '端拾器', '1700', '', '010703', '', 'A', '', '2019-04-07 22:15:31');
INSERT INTO `goods_info` VALUES ('146', 'B端拾器C211翼子板A转B', '端拾器', '1500', '', '010704', '', 'A', '', '2019-04-07 22:15:47');
INSERT INTO `goods_info` VALUES ('147', 'B端拾器S111前罩外板', '端拾器', '1500', '', '010705', '', 'A', '', '2019-04-07 22:16:07');
INSERT INTO `goods_info` VALUES ('148', 'A端拾器S111顶盖', '端拾器', '1300', '', '010706', '', 'A', '', '2019-04-07 22:16:21');
INSERT INTO `goods_info` VALUES ('149', '检具C201前门内板左-16', '检具', '1300', '', '010707', '', 'A', '', '2019-04-07 22:16:32');
INSERT INTO `goods_info` VALUES ('150', '检具托盘', '检具', '1800', '', '020101', '', 'A', '', '2019-04-08 01:03:07');
INSERT INTO `goods_info` VALUES ('151', 'A端拾器S111左侧围', '端拾器', '1700', '', '020102', '', 'A', '', '2019-04-08 01:03:26');
INSERT INTO `goods_info` VALUES ('152', '检具S101后门内板右-17', '检具', '1300', '', '020103', '', 'A', '', '2019-04-08 00:51:10');
INSERT INTO `goods_info` VALUES ('153', '检具C201翼子板（右）', '检具', '1300', '', '020104', '', 'A', '', '2019-04-08 01:04:08');
INSERT INTO `goods_info` VALUES ('154', '检具C201前面窗框左', '检具', '1300', '', '020105', '', 'A', '', '2019-04-08 00:55:17');
INSERT INTO `goods_info` VALUES ('155', '检具C201后门窗框左', '检具', '1300', '', '020106', '', 'A', '', '2019-04-08 00:55:48');
INSERT INTO `goods_info` VALUES ('156', '检具C201行李箱内板', '检具', '1300', '', '020107', '', 'A', '', '2019-04-08 00:58:17');
INSERT INTO `goods_info` VALUES ('157', '检具C201前门总成左（老款）', '检具', '1800', '', '020201', '', 'A', '', '2019-04-08 01:04:17');
INSERT INTO `goods_info` VALUES ('158', 'A端拾器S111翼子板', '端拾器', '1700', '', '020202', '', 'A', '', '2019-04-08 01:04:30');
INSERT INTO `goods_info` VALUES ('159', '检具托盘', '检具', '1300', '', '020203', '', 'A', '', '2019-04-08 00:58:52');
INSERT INTO `goods_info` VALUES ('160', '检具C201前门内板', '检具', '1300', '', '020204', '', 'A', '', '2019-04-08 00:59:11');
INSERT INTO `goods_info` VALUES ('161', '检具S101前门内板右', '检具', '1300', '', '020205', '', 'A', '', '2019-04-08 00:59:33');
INSERT INTO `goods_info` VALUES ('162', '检具C201后门窗框左', '检具', '1300', '', '020206', '', 'A', '', '2019-04-08 01:02:42');
INSERT INTO `goods_info` VALUES ('163', '检具托盘', '检具', '1300', '', '020207', '', 'A', '', '2019-04-08 01:05:02');
INSERT INTO `goods_info` VALUES ('164', '检具C201前门内板', '检具', '1800', '', '020301', '', 'A', '', '2019-04-08 01:05:29');
INSERT INTO `goods_info` VALUES ('165', 'B端拾器S101顶盖', '端拾器', '1700', '', '020302', '', 'A', '', '2019-04-08 01:06:24');
INSERT INTO `goods_info` VALUES ('166', '检具C201-15前罩外板', '检具', '1300', '', '020303', '', 'A', '', '2019-04-08 01:07:12');
INSERT INTO `goods_info` VALUES ('167', '检具S101前门外板左', '检具', '1300', '', '020304', '', 'A', '', '2019-04-08 01:08:04');
INSERT INTO `goods_info` VALUES ('168', '检具S111前门外板（右）', '检具', '1300', '', '020305', '', 'A', '', '2019-04-08 01:09:54');
INSERT INTO `goods_info` VALUES ('169', '检具C201前窗框右', '检具', '1300', '', '020306', '', 'A', '', '2019-04-08 01:09:45');
INSERT INTO `goods_info` VALUES ('170', '检具托盘', '检具', '1300', '', '020307', '', 'A', '', '2019-04-08 01:10:19');
INSERT INTO `goods_info` VALUES ('171', '检具S101翼子板右', '检具', '1800', '', '020401', '', 'A', '', '2019-04-08 01:11:30');
INSERT INTO `goods_info` VALUES ('172', 'B端拾器S101翼子板右', '端拾器', '1700', '', '020402', '', 'A', '', '2019-04-08 01:13:56');
INSERT INTO `goods_info` VALUES ('173', '检具C201后总成右-16', '检具', '1300', '', '020403', '', 'A', '', '2019-04-08 01:13:47');
INSERT INTO `goods_info` VALUES ('174', '检具S101前门内板（左）', '检具', '1300', '', '020404', '', 'A', '', '2019-04-08 01:13:25');
INSERT INTO `goods_info` VALUES ('175', '检具C201前门内板-14', '检具', '1300', '', '020405', '', 'A', '', '2019-04-08 01:17:06');
INSERT INTO `goods_info` VALUES ('176', '检具C201行李箱外块', '检具', '1300', '', '020406', '', 'A', '', '2019-04-08 01:17:00');
INSERT INTO `goods_info` VALUES ('177', '检具托盘', '检具', '1300', '', '020407', '', 'A', '', '2019-04-08 01:16:54');
INSERT INTO `goods_info` VALUES ('178', '检具托盘', '检具', '1800', '', '020501', '', 'A', '', '2019-04-08 01:16:51');
INSERT INTO `goods_info` VALUES ('179', 'B端拾器S101翼子板', '端拾器', '1700', '', '020502', '', 'A', '', '2019-04-08 01:16:47');
INSERT INTO `goods_info` VALUES ('180', '检具C201后门总成左-16', '检具', '1300', '', '020503', '', 'A', '', '2019-04-08 01:17:55');
INSERT INTO `goods_info` VALUES ('181', 'B端拾器S111前罩内板', '端拾器', '1300', '', '020504', '', 'A', '', '2019-04-08 01:18:17');
INSERT INTO `goods_info` VALUES ('182', 'B端拾器C201前罩外板U01', '端拾器', '1300', '', '020505', '', 'A', '', '2019-04-08 01:18:44');
INSERT INTO `goods_info` VALUES ('183', 'B端拾器C201前罩内板-U01', '端拾器', '1300', '', '020506', '', 'A', '', '2019-04-08 01:19:08');
INSERT INTO `goods_info` VALUES ('184', 'A端拾器S101后门内板', '端拾器', '1300', '', '020507', '', 'A', '', '2019-04-08 01:19:42');
INSERT INTO `goods_info` VALUES ('185', '检具托盘', '检具', '1800', '', '020601', '', 'A', '', '2019-04-08 01:19:50');
INSERT INTO `goods_info` VALUES ('186', 'A端拾器C201侧围右', '端拾器', '1700', '', '020602', '', 'A', '', '2019-04-08 01:20:22');
INSERT INTO `goods_info` VALUES ('187', '检具C201前门总成-右-16', '检具', '1300', '', '020603', '', 'A', '', '2019-04-08 01:21:03');
INSERT INTO `goods_info` VALUES ('188', 'B端拾器C201前罩内板U03', '端拾器', '1300', '', '020604', '', 'A', '', '2019-04-08 01:21:43');
INSERT INTO `goods_info` VALUES ('189', 'B端拾器S101前罩外板17款', '端拾器', '1300', '', '020605', '', 'A', '', '2019-04-08 01:22:29');
INSERT INTO `goods_info` VALUES ('190', 'A端拾器C211后门内板', '端拾器', '1300', '', '020606', '', 'A', '', '2019-04-08 01:22:47');
INSERT INTO `goods_info` VALUES ('191', 'A端拾器S101前门内板', '线端拾器', '1300', '', '020607', '', 'A', '', '2019-04-08 01:23:10');
INSERT INTO `goods_info` VALUES ('192', '检具S101背门总成老款', '检具', '1800', '', '020701', '', 'A', '', '2019-04-08 01:24:09');
INSERT INTO `goods_info` VALUES ('193', 'B端拾器S101侧围右', '端拾器', '1700', '', '020702', '', 'A', '', '2019-04-08 01:26:55');
INSERT INTO `goods_info` VALUES ('194', '检具C201前罩总成', '检具', '1300', '', '020703', '', 'A', '', '2019-04-08 01:26:58');
INSERT INTO `goods_info` VALUES ('195', 'B端拾器C201行李箱内板', '端拾器', '1300', '', '020704', '', 'A', '', '2019-04-08 01:27:02');
INSERT INTO `goods_info` VALUES ('196', 'B端拾器S111后门外板', '端拾器', '1300', '', '020705', '', 'A', '', '2019-04-08 01:27:06');
INSERT INTO `goods_info` VALUES ('197', 'B端拾器S101前罩内板-老款', '端拾器', '1300', '', '020706', '', 'A', '', '2019-04-08 01:27:09');
INSERT INTO `goods_info` VALUES ('198', 'A线端拾器C201后门内板', '端拾器', '1300', '', '020707', '', 'A', '', '2019-04-08 01:27:16');
