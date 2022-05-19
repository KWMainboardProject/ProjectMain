<?php
// 안드로이드에서 넘어온 데이터라고 가정하고 직접 DB에 데이터 저장 테스트
// 제대로 저장되는지 확인했으면 아래 4줄은 주석처리 또는 삭제해야 함

$_POST['userid'];
$_POST['Gender'];
$_POST['userpw'];
$_POST['Age'] = '24';
$_POST['LikeStyle'] = 'Dandy';



extract($_POST);

require_once 'config/db_functions.php';

$db = new DB_Functions();


// json response array
$response = array("error" => FALSE);

if (isset($_POST['userid']) && isset($_POST['Age']) && isset($_POST['gender']) && isset($_POST['userpw']) && isset($_POST['LikeStyle'])) {

    // POST 배열로 받은 데이터
    $userID = $_POST['userid'];
    $Age = $_POST['Age'];
    $Gender = $_POST['gender'];
    $password = $_POST['userpw'];
    $LikeStyle = $_POST['LikeStyle'];
    
    // 동일한 userID 등록되어 있는지 체크
    if ($db->isUserExisted($userID)) { // E-Mail 이 key value
        // user already existed
        $response["error"] = TRUE;
        $response["error_msg"] = "User already existed with " . $userID;
        echo json_encode($response);
    } else {
        // 사용자 등록
        
        $user = $db->storeUser($userID, $password, $LikeStyle, $Age, $Gender );
        
        if ($user) { // 사용자 등록 성공
            //storeUser($PID, $PPW, $LikeStyle, $Age, $Gender)
            $response['error'] = FALSE;
            $response['users']['PID'] = $user['PID'];
            $response['users']['PPW'] = $user['PPW'];
            $response['users']['LikeStyle'] = $user['LikeStyle'];
            $response['users']['Age'] = $user['Age'];
            $response['users']['Gender'] = $user['Gender'];

            echo $_POST["userid"] ."님 환영합니다.";
        } else {
            // 사용자 등록 실패
            $response['error'] = TRUE;
            $response['error_msg'] = "Unknown error occurred in registration!";
            echo json_encode($response);
        }
    }
} else { // 입력받은 데이터에 문제가 있을 경우
    $response['error'] = TRUE;
    $response['error_msg'] = "fail to registration because input data be incorrected";
    echo json_encode($response);
}
?>
<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
        <meta name="description" content="" />
        <meta name="author" content="" />
        <title>MainBoard Main Page</title>
        <!-- Favicon-->
        <link rel="icon" type="image/x-icon" href="assets/favicon.ico" />
        <!-- Bootstrap icons-->
        <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.5.0/font/bootstrap-icons.css" rel="stylesheet" />
        <!-- Core theme CSS (includes Bootstrap)-->
        <link href="css/styles.css" rel="stylesheet" />
        <link href="/Bootstrap/ProjectMain/webPage/additional_style.css" rel="stylesheet" />

        <!--AOS사용-->
        <link href="https://unpkg.com/aos@2.3.1/dist/aos.css" rel="stylesheet">
        <script src="https://unpkg.com/aos@2.3.1/dist/aos.js"></script> 
        <script src="https://code.jquery.com/jquery-1.11.3.min.js"></script> 
    </head>
    <body class="d-flex flex-column h-100">
        <script>
            AOS.init();
        </script>
        <main class="flex-shrink-0">
            <!-- Navigation-->
            <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
                <div class="container px-5">
                    <a class="navbar-brand" href="index.html">MainBoard</a>
                    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation"><span class="navbar-toggler-icon"></span></button>
                    <div class="collapse navbar-collapse" id="navbarSupportedContent">
                        <ul class="navbar-nav ms-auto mb-2 mb-lg-0">
                            <li class="nav-item"><a class="nav-link" href="index.html">Home</a></li>
                            <li class="nav-item"><a class="nav-link" href="about.html">About</a></li>
                            <li class="nav-item"><a class="nav-link" href="login.php">Login</a></li>
                            <li class="nav-item dropdown">
                                <a class="nav-link dropdown-toggle" id="navbarDropdownBlog" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">Collections</a>
                                <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="navbarDropdownBlog">
                                    <li><a class="dropdown-item" href="product-list.html">Outer</a></li>
                                    <li><a class="dropdown-item" href="product-list.html">Top</a></li>
                                    <li><a class="dropdown-item" href="product-list.html">Bottom</a></li>
                                    <li><a class="dropdown-item" href="product-list.html">Overall</a></li>
                                </ul>
                            </li>
                            <li class="nav-item"><a class="nav-link" href="product_create.html">Product Register</a></li>
                        </ul>
                    </div>
                </div>
            </nav>
            <!-- Header-->
            <header class="py-5" style="background-color: rgb(255, 166, 0);">
                <div class="container px-5" data-aos="fade-up" aos-offset="300" aos-easing="ease-in-sine" aos-duration="500">
                    <div class="row gx-5 align-items-center justify-content-center">
                        <div class="col-lg-8 col-xl-7 col-xxl-6">
                            <div class="my-5 text-center text-xl-start">
                                <h1 class="display-5 fw-bolder text-white mb-2">몇 번의 터치로<br> 원하는 옷을.<br></h1>
                                <p class="lead fw-normal text-white mb-4" style="font-size: 1em;">CNN 기반 YoloV5와 ResNet50을 사용한 유사 의류 추천.<br>찾고 싶은 옷의 사진만 있다면<br>원하는 옷과 빠르게 만날 수 있습니다.</p>
                                <div class="d-grid gap-3 d-sm-flex justify-content-sm-center justify-content-xl-start">
                                    <a class="btn btn-outline-light btn-lg px-4" href="about.html" style="background-color: rgb(0, 81, 255);">Learn More</a>
                                </div>
                            </div>
                        </div>
                        <!--Image Search Section-->
                        <div class="col-xl-5 col-xxl-6 d-xl-block text-center">
                            <div class="box" id="searchImg">
                                <img class="img-fluid rounded-3 my-5" src="/Bootstrap/ProjectMain/search.png" width="300" height="300" alt="Responsive image"/>
                                <div class="d-grid gap-3 d-sm-flex justify-content-sm-center justify-content-xl-start">
                                    <a class="btn btn-outline-light btn-lg px-4 me-sm-3" id="inner_btn" href="Image_input.html" style="background-color: rgb(0, 81, 255);">
                                        Get Started
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </header>
            <!-- Clothes Products section-->
            <section class="py-5" id="features">
                <div class="container px-5 my-5" data-aos="zoom-in">
                    <div class="row gx-5">
                        <div class="col-lg-4 mb-5 mb-lg-0"><h2 class="fw-bolder mb-0">실시간 상품 추천</h2></div>
                        <div class="col-lg-8">
                            <div class="row gx-5 row-cols-1 row-cols-md-2">
                                <div class="col mb-5 h-100">
                                    <button type="button" class="btn_image" id="img_btn1" style="border: 0;">
                                        <a href="product-info.html">
                                            <img src="/Bootstrap/ProjectMain/clothes1.jpg" style="width: 250px; height: 325px" class="cImg">
                                        </a>
                                    </button>
                                    <h2 class="h5">Open Collar Shirts</h2>
                                    <p class="mb-0">$135,000</p>
                                </div>
                                <div class="col mb-5 h-100">
                                    <button type="button" class="btn_image" id="img_btn2" style="border: 0;">
                                        <a href="product-info.html">
                                            <img src="/Bootstrap/ProjectMain/clothes2.jpg" style="width: 250px; height: 325px" class="cImg">
                                        </a>
                                    </button>
                                    <h2 class="h5">Nicholson Pants</h2>
                                    <p class="mb-0">$450,000</p>
                                </div>
                                <div class="col mb-5 mb-md-0 h-100">
                                    <button type="button" class="btn_image" id="img_btn3" style="border: 0;">
                                        <a href="product-info.html">
                                            <img src="/Bootstrap/ProjectMain/clothes3.jpg" style="width: 250px; height: 325px" class="cImg">
                                        </a>
                                    </button>
                                    <h2 class="h5">Flower Shirts</h2>
                                    <p class="mb-0">$179,000</p>
                                </div>
                                <div class="col h-100">
                                    <button type="button" class="btn_image" id="img_btn4" style="border: 0;">
                                        <a href="product-info.html">
                                            <img src="/Bootstrap/ProjectMain/clothes5.jpg" style="width: 250px; height: 325px" class="cImg">
                                        </a>
                                    </button>
                                    <h2 class="h5">Studio Nicholson Dove</h2>
                                    <p class="mb-0">$250,000</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </main>
        <!-- Footer-->
        <footer class="bg-dark py-4 mt-auto">
            <div class="container px-5">
                <div class="row align-items-center justify-content-between flex-column flex-sm-row">
                    <div class="col-auto"><div class="small m-0 text-white">Copyright &copy; MainBoard 2022</div></div>
                    <div class="col-auto">
                        <a class="link-light small" href="#!">Privacy</a>
                        <span class="text-white mx-1">&middot;</span>
                        <a class="link-light small" href="#!">Terms</a>
                        <span class="text-white mx-1">&middot;</span>
                        <a class="link-light small" href="#!">Contact</a>
                    </div>
                </div>
            </div>
        </footer>
        <!-- Bootstrap core JS-->
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
        <!-- Core theme JS-->
        <script src="js/scripts.js"></script>
    </body>
</html>

                    