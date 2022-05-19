<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
        <meta name="description" content="" />
        <meta name="author" content="" />
        <title>MainBoard Product Information Page</title>
        <!-- Favicon-->
        <link rel="icon" type="image/x-icon" href="assets/favicon.ico" />
        <!-- Bootstrap icons-->
        <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.5.0/font/bootstrap-icons.css" rel="stylesheet" />
        <!-- Core theme CSS (includes Bootstrap)-->
        <link href="css/styles.css" rel="stylesheet" />

        <script src="example.js"></script>
    </head>
    <body class="d-flex flex-column h-100">
        <main class="flex-shrink-0">
            <!-- Navigation-->
            <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
                <div class="container px-5">
                    <a class="navbar-brand" href="index.php">MainBoard</a>
                    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation"><span class="navbar-toggler-icon"></span></button>
                    <div class="collapse navbar-collapse" id="navbarSupportedContent">
                        <ul class="navbar-nav ms-auto mb-2 mb-lg-0">
                            <li class="nav-item"><a class="nav-link" href="index.php">Home</a></li>
                            <li class="nav-item"><a class="nav-link" href="about.php">About</a></li>
                            <li class="nav-item"><a class="nav-link" href="login.php">Login</a></li>
                            <li class="nav-item dropdown">
                                <a class="nav-link dropdown-toggle" id="navbarDropdownBlog" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">Collections</a>
                                <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="navbarDropdownBlog">
                                    <li><a class="dropdown-item" href="Outer-list.php">Outer</a></li>
                                    <li><a class="dropdown-item" href="Top-list.php">Top</a></li>
                                    <li><a class="dropdown-item" href="Bottom-list.php">Bottom</a></li>
                                    <li><a class="dropdown-item" href="Overall-list.php">Overall</a></li>
                                </ul>
                            </li>
                            <li class="nav-item"><a class="nav-link" href="product_create.php">Product Register</a></li>
                            <li name="nav_myproducts" class="nav-item"><a class="nav-link" href="myProduct-list.php" style="display: none;">My Products</a></li>
                        </ul>
                    </div>
                </div>
            </nav>
            <!-- Page Content-->
            <section class="py-5">
                <div class="container">
                    <div class="row">
                        <!--product images-->
                        <div class="col-lg-5 mb-3">
                            <div id="carouselExampleInterval" class="carousel carousel-dark slide" data-bs-ride="carousel">
                                <!--상품 이미지들(썸네일 이미지 아님!)-->
                                <div class="carousel-inner">
                                    <!--각 이미지 for문 통해서 DB에서 가져와야함.-->
                                  <div class="carousel-item active" data-bs-interval="10000">
                                    <img name="productImg1" src="" class="d-block w-100" alt="..." style="width: 600px; height: 800px; object-fit: contain;">
                                  </div>
                                  <div class="carousel-item" data-bs-interval="2000">
                                    <img name="productImg2" src="" class="d-block w-100" alt="..." style="width: 600px; height: 800px; object-fit: contain;">
                                  </div>
                                  <div class="carousel-item">
                                    <img name="productImg3" src="" class="d-block w-100" alt="..." style="width: 600px; height: 800px; object-fit: contain;">
                                  </div>
                                </div>
                                <button class="carousel-control-prev" type="button" data-bs-target="#carouselExampleInterval" data-bs-slide="prev">
                                  <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                                  <span class="visually-hidden">Previous</span>
                                </button>
                                <button class="carousel-control-next" type="button" data-bs-target="#carouselExampleInterval" data-bs-slide="next">
                                  <span class="carousel-control-next-icon" aria-hidden="true"></span>
                                  <span class="visually-hidden">Next</span>
                                </button>
                            </div>
                        </div>
                        <!--product informations-->
                        <div class="col-lg-7 mt-5">
                            <div class="card shadow-sm">
                                <div class="card-body">
                                    <!--상품 정보 기입 파트 / 상품명, 상품 서브 카테고리, 상품 가격, 상품 상세 설명 DB에서 가져오기-->
                                    <h2 name="productName" class="card-title fw-bolder">Product Name(DB)</h5>
                                    <h5 name="productSubCat" class="card-title pt-3 pb-3">Product SubCategory(DB)</h5>
                                    <h5 name="productPrice" class="card-title pt-3 pb-3 border-top">Product Price(DB)</h5>
                                    <!--이 h5 태그는 DB연동하는 부분 아님.-->
                                    <h5 class="card-title pt-3 pb-3 border-top">Product Description</h5>
                                    <!--상품 상세 정보는 여기 p태그 수정하기-->
                                    <p name="productInfo">상품 상세 설명 DB에서 가져오기</p>
                                    <div class="d-flex justify-content-between align-items-center">
                                        <div class="col-12 d-grid p-1">
                                            <button type="button" class="btn btn-lg btn-outline-dark">Buy</button>
                                        </div>
                                    </div>
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
