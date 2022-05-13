import json
import os
from abc import *

class DictUseAbleContainer(metaclass=ABCMeta):
    @abstractmethod
    def getDict(self):
        #return dict_container
        pass
    @abstractmethod
    def getDict(self, container:dict):
        #return None
        #set inner data
        pass
    @abstractmethod
    def getKeyName(self):
        #return root_key
        pass

class JsonPath(DictUseAbleContainer):
    def __init__(self, p):
        self.__json_path = p

    def setPath(self, p):
        if not os.path.isdir(p):
            os.makedirs(p)
        self.__json_path = p

    def getPath(self):
        return self.__json_path


class ConfidenceContainer(DictUseAbleContainer):
    def __init__(self, t, c=0):
        self.__confidence = c
        self.__threshold = t

    def setConfidence(self, c):
        self.__confidence = c


class Category(DictUseAbleContainer):
    def __init__(self, index=-1):
        self.__label_list = []
        self.__index = index

    def setIndex(self, index):
        self.__index = index

    def setCategory(self, label_list):
        self.__label_list = label_list

    def getCategory(self):
        return self.__label_list


class SaveImageContainer(DictUseAbleContainer):
    def __init__(self, file_path):
        self.__img_path = ''
        self.__save_path = JsonPath(file_path)

    def addImagePath(self, img_path):
        self.__img_path = img_path


class ColorContainer(DictUseAbleContainer):
    def __init__(self, file_path):
        self.__color_dict = {}
        self.__save_path = JsonPath(file_path)

    # 입력에 대한 구체화 필요
    def addColor(self, color_dict):
        self.__color_dict = color_dict


class StyleContainer(DictUseAbleContainer):
    def __init__(self, id, file_path=""):
        self.__ID = id
        self.__j_data = {}
        self.__label = Category()
        self.__save_path = JsonPath(file_path)  # json file 생성 로직에 따라 수정 가능
        self. __confidence = ConfidenceContainer(0)

    def getDict(self):
        with open(self.__save_path.getPath() + "/ST_" + str(self.__ID) + ".json", 'r') as json_file:
            return json.load(json_file)

    def setDict(self):
        with open(self.__save_path.getPath() + "/ST_" + str(self.__ID) + ".json", 'w') as save_file:
            json.dump({"ST": {"label": self.__label.getCategory()}}, save_file)

    def setConfidence(self, c):
        self.__confidence.setConfidence(c)

    def setIndex(self, index):
        self.__label.setIndex(index)

    def setLabel(self, label):
        self.__label.setCategory(label)

    def setPath(self, p):
        self.__save_path.setPath(p)

    def getPath(self):
        return self.__save_path.getPath()


class PatternContainer(DictUseAbleContainer):
    def __init__(self, id, file_path=""):
        self.__ID = id
        self.__j_data = {}
        self.__label = Category()
        self.__save_path = JsonPath(file_path)  # json file 생성 로직에 따라 수정 가능
        self. __confidence = ConfidenceContainer(0)

    def getDict(self):
        with open(self.__save_path.getPath() + "/PC_" + str(self.__ID) + ".json", 'r') as json_file:
            return json.load(json_file)

    def setDict(self):
        with open(self.__save_path.getPath() + "/PC_" + str(self.__ID) + ".json", 'w') as save_file:
            json.dump({"PC": {"label": self.__label.getCategory()}}, save_file)

    def setConfidence(self, c):
        self.__confidence.setConfidence(c)

    def setIndex(self, index):
        self.__label.setIndex(index)

    def setLabel(self, label):
        self.__label.setCategory(label)

    def setPath(self, p):
        self.__save_path.setPath(p)

    def getPath(self):
        return self.__save_path.getPath()


class SubCategoryContainer(DictUseAbleContainer):
    def __init__(self, id, file_path=""):
        self.__ID = id
        self.__j_data = {}
        self.__label = Category()
        self.__save_path = JsonPath(file_path)  # json file 생성 로직에 따라 수정 가능
        self. __confidence = ConfidenceContainer(0)

    def getDict(self):
        with open(self.__save_path.getPath() + "/SC_" + str(self.__ID) + ".json", 'r') as json_file:
            return json.load(json_file)

    def setDict(self):
        with open(self.__save_path.getPath() + "/SC_" + str(self.__ID) + ".json", 'w') as save_file:
            json.dump({"SC": {"label": self.__label.getCategory()}}, save_file)

    def setConfidence(self, c):
        self.__confidence.setConfidence(c)

    def setIndex(self, index):
        self.__label.setIndex(index)

    def setLabel(self, label):
        self.__label.setCategory(label)

    def setPath(self, p):
        self.__save_path.setPath(p)

    def getPath(self):
        return self.__save_path.getPath()


class MainCategoryContainer(DictUseAbleContainer):
    def __init__(self, id, file_path=""):
        self.__ID = id
        self.__j_data = {}
        self.__label = Category()
        self.__save_path = JsonPath(file_path)   # json file 생성 로직에 따라 수정 가능
        self.__confidence = ConfidenceContainer(0)
        self.__bound_box = [0, 0, 0, 0]  # x_min, x_max, y_min, y_max

    def getDict(self):
        with open(self.__save_path.getPath() + "/MC_" + str(self.__ID) + ".json", 'r') as json_file:
            return json.load(json_file)

    def setDict(self):
        with open(self.__save_path.getPath() + "/MC_" + str(self.__ID) + ".json", 'w') as save_file:
            json.dump({"MC": {"label": self.__label.getCategory(), "Box": self.__bound_box}}, save_file)

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

    def setPath(self, p):
        self.__save_path.setPath(p)

    def getPath(self):
        return self.__save_path.getPath()


class FashionContainer(DictUseAbleContainer):
    def __init__(self, id, file_path):
        self.__ID = id
        self.__j_data = {"ID": id}
        self.__save_path = JsonPath(file_path)

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

    def setDict(self):
        with open(self.__save_path.getPath() + "/" + str(self.__ID) + ".json", 'w') as save_file:
            json.dump(self.__j_data, save_file)

    def setPath(self, p):
        self.__save_path.setPath(p)

    def getPath(self):
        return self.__save_path.getPath()
