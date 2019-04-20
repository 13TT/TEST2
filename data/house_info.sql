/*
Navicat MySQL Data Transfer

Source Server         : database_lbt
Source Server Version : 50561
Source Host           : 127.0.0.1:3306
Source Database       : db_cawc

Target Server Type    : MYSQL
Target Server Version : 50561
File Encoding         : 65001

Date: 2019-04-12 13:08:41
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for house_info
-- ----------------------------
DROP TABLE IF EXISTS `house_info`;
CREATE TABLE `house_info` (
  `house_number` varchar(255) NOT NULL,
  `house_state` char(1) DEFAULT 'N',
  `bf_h_to_h` varchar(10) DEFAULT '',
  `bf_h_to_h_2` char(1) DEFAULT '',
  `before_change` char(1) DEFAULT '',
  `house_style` varchar(10) DEFAULT 'A',
  `line` tinyint(255) DEFAULT '0',
  `list` tinyint(255) DEFAULT '0',
  `blank` tinyint(255) DEFAULT '0',
  `floor` tinyint(255) DEFAULT '0',
  `modify_time` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`house_number`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Records of house_info
-- ----------------------------
INSERT INTO `house_info` VALUES ('010101', 'N', '1700', '', 'N', '端拾器', '1', '1', '1', '1', '2019-04-12 11:57:53');
INSERT INTO `house_info` VALUES ('010102', 'N', '1700', '', 'N', '检具', '1', '1', '1', '2', '2019-04-12 11:58:08');
INSERT INTO `house_info` VALUES ('010103', 'N', '1700', '', 'N', '端拾器', '1', '1', '1', '3', '2019-04-12 11:58:29');
INSERT INTO `house_info` VALUES ('010104', 'S', '1300', '', 'N', '检具', '1', '1', '1', '4', '2019-04-11 10:59:44');
INSERT INTO `house_info` VALUES ('010105', 'S', '1300', '', 'N', '检具', '1', '1', '1', '5', '2019-04-11 11:24:02');
INSERT INTO `house_info` VALUES ('010106', 'S', '1300', '', 'N', '检具', '1', '1', '1', '6', '2019-04-12 11:30:56');
INSERT INTO `house_info` VALUES ('010107', 'N', '1300', '', 'N', '检具', '1', '1', '1', '7', '2019-04-12 11:59:30');
INSERT INTO `house_info` VALUES ('010201', 'S', '1700', '', 'N', '端拾器', '1', '1', '2', '1', '2019-04-12 11:49:58');
INSERT INTO `house_info` VALUES ('010202', 'S', '1700', '', 'N', '检具', '1', '1', '2', '2', '2019-04-12 11:51:45');
INSERT INTO `house_info` VALUES ('010203', 'S', '1700', '', 'N', '端拾器', '1', '1', '2', '3', '2019-04-12 11:54:24');
INSERT INTO `house_info` VALUES ('010204', 'S', '1300', '', 'N', '检具', '1', '1', '2', '4', '2019-04-12 11:54:52');
INSERT INTO `house_info` VALUES ('010205', 'S', '1300', '', 'N', '检具', '1', '1', '2', '5', '2019-04-12 11:56:55');
INSERT INTO `house_info` VALUES ('010206', 'N', '1300', '', 'N', '检具', '1', '1', '2', '6', '2019-04-09 15:17:28');
INSERT INTO `house_info` VALUES ('010207', 'N', '1300', '', 'N', '检具', '1', '1', '2', '7', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010301', 'X', '1700', '', 'N', '端拾器', '1', '1', '3', '1', '2019-04-10 21:00:47');
INSERT INTO `house_info` VALUES ('010302', 'X', '1700', '', 'N', '检具', '1', '1', '3', '2', '2019-04-10 21:00:51');
INSERT INTO `house_info` VALUES ('010303', 'N', '1700', '', 'N', '端拾器', '1', '1', '3', '3', '2019-04-11 16:26:39');
INSERT INTO `house_info` VALUES ('010304', 'N', '1300', '', 'N', '检具', '1', '1', '3', '4', '2019-04-09 15:17:32');
INSERT INTO `house_info` VALUES ('010305', 'N', '1300', '', 'N', '检具', '1', '1', '3', '5', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010306', 'N', '1300', '', 'N', '检具', '1', '1', '3', '6', '2019-04-08 09:37:38');
INSERT INTO `house_info` VALUES ('010307', 'N', '1300', '', 'N', '检具', '1', '1', '3', '7', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010401', 'N', '1700', '', 'N', '端拾器', '1', '1', '4', '1', '2019-04-09 15:17:36');
INSERT INTO `house_info` VALUES ('010402', 'N', '1700', '', 'N', '检具', '1', '1', '4', '2', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010403', 'N', '1700', '', 'N', '端拾器', '1', '1', '4', '3', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010404', 'N', '1300', '', 'N', '检具', '1', '1', '4', '4', '2019-04-08 09:26:52');
INSERT INTO `house_info` VALUES ('010405', 'N', '1300', '', 'N', '检具', '1', '1', '4', '5', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010406', 'N', '1300', '', 'N', '检具', '1', '1', '4', '6', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010407', 'N', '1300', '', 'N', '检具', '1', '1', '4', '7', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010501', 'N', '1700', '', 'N', '端拾器', '1', '1', '5', '1', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010502', 'N', '1700', '', 'N', '检具', '1', '1', '5', '2', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010503', 'N', '1700', '', 'N', '端拾器', '1', '1', '5', '3', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010504', 'N', '1500', '', 'N', '端拾器', '1', '1', '5', '4', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010505', 'N', '1500', '', 'N', '端拾器', '1', '1', '5', '5', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010506', 'N', '1300', '', 'N', '端拾器', '1', '1', '5', '6', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010507', 'N', '1300', '', 'N', '检具', '1', '1', '5', '7', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010601', 'N', '1700', '', 'N', '端拾器', '1', '1', '6', '1', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010602', 'N', '1700', '', 'N', '检具', '1', '1', '6', '2', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010603', 'N', '1700', '', 'N', '端拾器', '1', '1', '6', '3', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010604', 'N', '1500', '', 'N', '端拾器', '1', '1', '6', '4', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010605', 'N', '1500', '', 'N', '端拾器', '1', '1', '6', '5', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010606', 'N', '1300', '', 'N', '端拾器', '1', '1', '6', '6', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010607', 'N', '1300', '', 'N', '检具', '1', '1', '6', '7', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010701', 'N', '1700', '', 'N', '端拾器', '1', '1', '7', '1', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010702', 'N', '1700', '', 'N', '检具', '1', '1', '7', '2', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010703', 'N', '1700', '', 'N', '端拾器', '1', '1', '7', '3', '2019-04-08 08:37:39');
INSERT INTO `house_info` VALUES ('010704', 'N', '1500', '', 'N', '端拾器', '1', '1', '7', '4', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010705', 'N', '1500', '', 'N', '端拾器', '1', '1', '7', '5', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010706', 'N', '1300', '', 'N', '端拾器', '1', '1', '7', '6', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('010707', 'N', '1300', '', 'N', '检具', '1', '1', '7', '7', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020101', 'N', '1800', '', 'N', '检具', '1', '2', '1', '1', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020102', 'N', '1700', '', 'N', '端拾器', '1', '2', '1', '2', '2019-04-09 17:12:32');
INSERT INTO `house_info` VALUES ('020103', 'N', '1300', '', 'N', '检具', '1', '2', '1', '3', '2019-04-10 19:31:12');
INSERT INTO `house_info` VALUES ('020104', 'N', '1300', '', 'N', '检具', '1', '2', '1', '4', '2019-04-10 19:31:13');
INSERT INTO `house_info` VALUES ('020105', 'N', '1300', '', 'N', '检具', '1', '2', '1', '5', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020106', 'N', '1300', '', 'N', '检具', '1', '2', '1', '6', '2019-04-08 08:52:54');
INSERT INTO `house_info` VALUES ('020107', 'N', '1300', '', 'N', '检具', '1', '2', '1', '7', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020201', 'N', '1800', '', 'N', '检具', '1', '2', '2', '1', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020202', 'N', '1700', '', 'N', '端拾器', '1', '2', '2', '2', '2019-04-09 16:55:08');
INSERT INTO `house_info` VALUES ('020203', 'N', '1300', '', 'N', '检具', '1', '2', '2', '3', '2019-04-10 19:31:01');
INSERT INTO `house_info` VALUES ('020204', 'N', '1300', '', 'N', '检具', '1', '2', '2', '4', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020205', 'N', '1300', '', 'N', '检具', '1', '2', '2', '5', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020206', 'N', '1300', '', 'N', '检具', '1', '2', '2', '6', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020207', 'N', '1300', '', 'N', '检具', '1', '2', '2', '7', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020301', 'X', '1800', '', 'N', '检具', '1', '2', '3', '1', '2019-04-10 19:31:19');
INSERT INTO `house_info` VALUES ('020302', 'X', '1700', '', 'N', '端拾器', '1', '2', '3', '2', '2019-04-10 19:31:18');
INSERT INTO `house_info` VALUES ('020303', 'X', '1300', '', 'N', '检具', '1', '2', '3', '3', '2019-04-10 19:31:14');
INSERT INTO `house_info` VALUES ('020304', 'X', '1300', '', 'N', '检具', '1', '2', '3', '4', '2019-04-10 19:31:15');
INSERT INTO `house_info` VALUES ('020305', 'X', '1300', '', 'N', '检具', '1', '2', '3', '5', '2019-04-10 19:31:17');
INSERT INTO `house_info` VALUES ('020306', 'N', '1300', '', 'N', '检具', '1', '2', '3', '6', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020307', 'N', '1300', '', 'N', '检具', '1', '2', '3', '7', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020401', 'N', '1800', '', 'N', '检具', '1', '2', '4', '1', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020402', 'N', '1700', '', 'N', '端拾器', '1', '2', '4', '2', '2019-04-09 19:08:18');
INSERT INTO `house_info` VALUES ('020403', 'N', '1300', '', 'N', '检具', '1', '2', '4', '3', '2019-04-10 19:31:02');
INSERT INTO `house_info` VALUES ('020404', 'N', '1300', '', 'N', '检具', '1', '2', '4', '4', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020405', 'N', '1300', '', 'N', '检具', '1', '2', '4', '5', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020406', 'N', '1300', '', 'N', '检具', '1', '2', '4', '6', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020407', 'N', '1300', '', 'N', '检具', '1', '2', '4', '7', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020501', 'N', '1800', '', 'N', '检具', '1', '2', '5', '1', '2019-04-09 15:17:43');
INSERT INTO `house_info` VALUES ('020502', 'N', '1700', '', 'N', '端拾器', '1', '2', '5', '2', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020503', 'N', '1300', '', 'N', '检具', '1', '2', '5', '3', '2019-04-10 19:31:05');
INSERT INTO `house_info` VALUES ('020504', 'N', '1300', '', 'N', '端拾器', '1', '2', '5', '4', '2019-04-09 15:17:43');
INSERT INTO `house_info` VALUES ('020505', 'N', '1300', '', 'N', '端拾器', '1', '2', '5', '5', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020506', 'N', '1300', '', 'N', '端拾器', '1', '2', '5', '6', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020507', 'N', '1300', '', 'N', '端拾器', '1', '2', '5', '7', '2019-04-09 15:17:42');
INSERT INTO `house_info` VALUES ('020601', 'N', '1800', '', 'N', '检具', '1', '2', '6', '1', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020602', 'N', '1700', '', 'N', '端拾器', '1', '2', '6', '2', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020603', 'N', '1300', '', 'N', '检具', '1', '2', '6', '3', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020604', 'N', '1300', '', 'N', '端拾器', '1', '2', '6', '4', '2019-04-09 15:17:37');
INSERT INTO `house_info` VALUES ('020605', 'N', '1300', '', 'N', '端拾器', '1', '2', '6', '5', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020606', 'N', '1300', '', 'N', '端拾器', '1', '2', '6', '6', '2019-04-09 15:17:40');
INSERT INTO `house_info` VALUES ('020607', 'N', '1300', '', 'N', '端拾器', '1', '2', '6', '7', '2019-04-09 15:17:38');
INSERT INTO `house_info` VALUES ('020701', 'N', '1800', '', 'N', '检具', '1', '2', '7', '1', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020702', 'N', '1700', '', 'N', '端拾器', '1', '2', '7', '2', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020703', 'N', '1300', '', 'N', '检具', '1', '2', '7', '3', '2019-04-09 15:17:39');
INSERT INTO `house_info` VALUES ('020704', 'N', '1300', '', 'N', '端拾器', '1', '2', '7', '4', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020705', 'N', '1300', '', 'N', '端拾器', '1', '2', '7', '5', '2019-04-09 16:27:33');
INSERT INTO `house_info` VALUES ('020706', 'N', '1300', '', 'N', '端拾器', '1', '2', '7', '6', '2019-04-09 15:17:41');
INSERT INTO `house_info` VALUES ('020707', 'N', '1300', '', 'N', '端拾器', '1', '2', '7', '7', '2019-04-09 16:27:33');
