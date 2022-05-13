import json
import os
from abc import *


class DictUseAbleContainer(metaclass=ABCMeta):
    @abstractmethod
    def getDict(self):
        #return dict_container
        pass

    @abstractmethod
    def setDict(self, container: dict):
        #return None
        #set inner data
        pass

    """@abstractmethod
    def getKeyName(self):
        #return root_key
        pass"""


class JsonPath():
    def __init__(self, p):
        if p[-5:] == ".json":
            dir = ""
            for dirs in p[:-5].split(sep="/")[:-1]:
                dir += dirs + "/"
            if not os.path.exists(dir):
                os.makedirs(dir)
        self.__json_path = p

    def setPath(self, p):
        if p[-5:] == ".json":
            dir = ""
            for dirs in p[:-5].split(sep="/"):
                dir += dirs + "/"
            if os.path.exists(dir):
                os.makedirs(dir)
        self.__json_path = p

    def getPath(self):
        return self.__json_path


class ConfidenceContainer():
    def __init__(self, t, c=0):
        self.__confidence = c
        self.__threshold = t

    def setConfidence(self, c):
        self.__confidence = c


class Category():
    def __init__(self, index=-1):
        self.__label_list = []
        self.__index = index

    def setIndex(self, index):
        self.__index = index

    def setCategory(self, label_list):
        self.__label_list = label_list

    def getCategory(self):
        return self.__label_list


class SaveImageContainer():
    def __init__(self, file_path):
        self.__img_path = ''
        self.__save_path = JsonPath(file_path)

    def addImagePath(self, img_path):
        self.__img_path = img_path


class ColorContainer():
    def __init__(self):
        self.__color_dict = {}
        self.__save_path = JsonPath()

    # 입력에 대한 구체화 필요
    def addColor(self, color_dict):
        self.__color_dict = color_dict


class StyleContainer(DictUseAbleContainer):
    def __init__(self, id):
        self.__ID = id
        self.__j_data = {}
        self.__label = Category()
        self. __confidence = ConfidenceContainer(0)

    def getDict(self):
        return self.__j_data

    def setDict(self, dict):
        self.__j_data = dict

    def setConfidence(self, c):
        self.__confidence.setConfidence(c)

    def setIndex(self, index):
        self.__label.setIndex(index)

    def setLabel(self, label):
        self.__label.setCategory(label)


class PatternContainer(DictUseAbleContainer):
    def __init__(self, id):
        self.__ID = id
        self.__j_data = {}
        self.__label = Category()
        self. __confidence = ConfidenceContainer(0)

    def getDict(self):
        return self.__j_data

    def setDict(self, dict):
        self.__j_data = dict

    def setConfidence(self, c):
        self.__confidence.setConfidence(c)

    def setIndex(self, index):
        self.__label.setIndex(index)

    def setLabel(self, label):
        self.__label.setCategory(label)


class SubCategoryContainer(DictUseAbleContainer):
    def __init__(self, id):
        self.__ID = id
        self.__j_data = {}
        self.__label = Category()
        self. __confidence = ConfidenceContainer(0)

    def getDict(self):
        return self.__j_data

    def setDict(self, dict):
        self.__j_data = dict

    def setConfidence(self, c):
        self.__confidence.setConfidence(c)

    def setIndex(self, index):
        self.__label.setIndex(index)

    def setLabel(self, label):
        self.__label.setCategory(label)


class MainCategoryContainer(DictUseAbleContainer):
    def __init__(self, id):
        self.__ID = id
        self.__j_data = {}
        self.__label = Category()
        self.__confidence = ConfidenceContainer(0)
        self.__bound_box = [0, 0, 0, 0]  # x_min, x_max, y_min, y_max

    def getDict(self):
        return self.__j_data

    def setDict(self, dict):
        self.__j_data = dict

    def setBox(self, list):
        self.__bound_box = list

    def getBox(self):
        return self.__bound_box

    def setConfidence(self, c):
        self.__confidence.setConfidence(c)

    def setIndex(self, index):
        self.__label.setIndex(index)

    def setLabel(self, label):
        self.__label.setCategory(label)


class FashionContainer(DictUseAbleContainer):
    def __init__(self, id):
        self.__ID = id
        self.__j_data = {}

        # setPath 필수
        self.mc = MainCategoryContainer(self.__ID)
        self.sc = SubCategoryContainer(self.__ID)
        self.pc = PatternContainer(self.__ID)
        self.st = StyleContainer(self.__ID)

    def getDict(self):
        for key, value in self.mc.getDict().items():
            self.__j_data[key] = value
        for key, value in self.sc.getDict().items():
            self.__j_data[key] = value
        for key, value in self.pc.getDict().items():
            self.__j_data[key] = value
        for key, value in self.st.getDict().items():
            self.__j_data[key] = value
        return self.__j_data

    def setDict(self, dict):
        self.__j_data = dict
