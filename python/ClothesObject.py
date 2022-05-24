import os
ENCODE_METHOD = 'utf-8'

def Convert_xywh_to_xyxy(xywh):
    start_x = xywh[0]
    start_y = xywh[1]
    w = xywh[2]
    h = xywh[3]

    xmin = start_x
    ymin = start_y
    xmax = xmin + w
    ymax = ymin + h
    return [xmin, ymin, xmax, ymax]

class Bound_box:
    def __init__(self, xyxy, class_idx, confidence):
        self.box['xmin'] = int(xyxy[0])
        self.box['ymin'] = int(xyxy[1])
        self.box['xmax'] = int(xyxy[2])
        self.box['ymax'] = int(xyxy[3])
        self.box['class'] = int(class_idx)
        self.box['confidence'] = confidence

    def __init__(self, bnd_box: dict):
        self.box = bnd_box

    def __str__(self):
        return self.box.__str__()

    def Get_xyxy(self):
        return self.box

    def Get_xywh(self):
        x_min = self.box['xmin']
        x_max = self.box['xmax']
        y_min = self.box['ymin']
        y_max = self.box['ymax']

        xywhBox = dict()
        xywhBox['x_center'] = int((x_min + x_max) / 2)
        xywhBox['y_center'] = int((y_min + y_max) / 2)
        xywhBox['w'] = int((x_max - x_min))
        xywhBox['h'] = int((y_max - y_min))
        xywhBox['class'] = self.box['class']
        xywhBox['confidence'] = self.box['confidence']
        return xywhBox

    def Get_xywh_nomalized(self, img_size: list):
        x_min = self.box['xmin']
        x_max = self.box['xmax']
        y_min = self.box['ymin']
        y_max = self.box['ymax']

        x_center = float((x_min + x_max)) / 2 / img_size[1]
        y_center = float((y_min + y_max)) / 2 / img_size[0]

        w = float((x_max - x_min)) / img_size[1]
        h = float((y_max - y_min)) / img_size[0]
        class_idx = self.box['class']

        return class_idx, x_center, y_center, w, h


class Objects_in_image:
    def __init__(self, img_size=list(), obj_in_img=None):
        self.objects = list()
        isinstance(obj_in_img, Objects_in_image)
        if obj_in_img is None:
            self.img_size = img_size
        elif  isinstance(obj_in_img, Objects_in_image) == True:
            self.img_size = obj_in_img.img_size[:-1]
            for obj in obj_in_img.objects:
                self.objects.append(obj)

    def Add_bound_box(self, xyxy :list, class_idx, confidence):
        bnd_box = dict()
        bnd_box['xmin'] = int(xyxy[0])
        bnd_box['ymin'] = int(xyxy[1])
        bnd_box['xmax'] = int(xyxy[2])
        bnd_box['ymax'] = int(xyxy[3])
        bnd_box['class'] = int(class_idx)
        bnd_box['confidence'] = confidence
        self.objects.append(bnd_box)

    def __str__(self):
        s = "img_size{width(%d), height(%d)}\n" %(self.img_size[1], self.img_size[0])
        s += "[\n"
        for line in self.objects:
            s += '\t' + dict(line).__str__() + "\n"
        s += "]"
        return s

    def __len__(self):
        return self.objects.__len__()

    def Get_item(self, idx):
        try:
            bnd_box = Bound_box(self.objects[idx])
        except IndexError as e:
            return "out of index"
        return bnd_box

    def Get_img_size(self):
        width = self.img_size[0]
        height =  self.img_size[1]
        channel = self.img_size[2]
        return width, height, channel

    def Save_object_to_yolo(self, filename):
        with open(filename, 'w', encoding=ENCODE_METHOD) as f:
            for idx in range(0, self.__len__()):
                obj = self.Get_item(idx)
                class_index, x_center, y_center, w, h = obj.Get_xywh_nomalized(self.img_size)
                f.write("%d %.6f %.6f %.6f %.6f\n" % (class_index, x_center, y_center, w, h))




class Clothes_in_object(Objects_in_image):
    class_list = ["overall", "bottom", "top", "outer", "shoes"]
    kor_class_dict = dict(아우터="outer", 하의="bottom", 상의="top", 원피스="overall", 신발="shoes")
    class_kor_dict = dict(outer="아우터", bottom="하의", top="상의", overall="원피스", shoes="신발")

    def Add_bound_box(self, xyxy :list, class_idx, confidence):
        bnd_box = dict()
        bnd_box['xmin'] = int(xyxy[0])
        bnd_box['ymin'] = int(xyxy[1])
        bnd_box['xmax'] = int(xyxy[2])
        bnd_box['ymax'] = int(xyxy[3])
        bnd_box['class'] = int(class_idx)
        bnd_box['confidence'] = confidence
        self.objects.append(bnd_box)

    def __str__(self):
        s = "img_size{width(%d), height(%d)}\n" %(self.img_size[1], self.img_size[0])
        s += "[\n"
        for line in self.objects.copy():
            k1=int(line['class'])
            k2=str(self.class_list[k1])
            k3=self.class_kor_dict[k2]
            kor_class = k3
            line['class'] = kor_class
            s += '\t' + dict(line).__str__() + "\n"
        s += "]"
        return s

    def Convert_korean_to_idx(self, korean):
        eng = self.kor_class_dict[korean]
        return self.class_list.index(eng)

    def Save_class_file_to_yolo(self, dir_path):
        classes_file = os.path.join(dir_path, "classes.txt")#os.path.dirname(os.path.abspath(filename)), "classes.txt")
        with open(classes_file, 'w', encoding=ENCODE_METHOD) as f:
            for label in self.class_list:
                f.write("%s\n" %(label))

