<?php
/**
 * ===========================
 * 404 에러 페이지 (404 Error Page)
 * ===========================
 *
 * 존재하지 않는 페이지 요청 시 표시
 * Displayed when requesting non-existent page
 */

$pageTitle = '페이지를 찾을 수 없습니다';
include LAYOUT_PATH . '/header.php';
?>

<style>
body {
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    color: white;
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 100vh;
    margin: 0;
}

.error-container {
    text-align: center;
    padding: 60px 40px;
    background: rgba(255, 255, 255, 0.1);
    border-radius: 20px;
    backdrop-filter: blur(10px);
    box-shadow: 0 8px 32px rgba(0, 0, 0, 0.3);
    max-width: 600px;
}

.error-code {
    font-size: 120px;
    font-weight: bold;
    margin: 0;
    line-height: 1;
    text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.2);
}

.error-title {
    font-size: 32px;
    margin: 20px 0;
}

.error-description {
    font-size: 18px;
    margin: 20px 0;
    opacity: 0.9;
}

.btn-home {
    display: inline-block;
    margin-top: 30px;
    padding: 16px 40px;
    background: white;
    color: #667eea;
    text-decoration: none;
    border-radius: 12px;
    font-weight: bold;
    font-size: 18px;
    transition: all 0.3s;
    box-shadow: 0 4px 16px rgba(0, 0, 0, 0.2);
}

.btn-home:hover {
    transform: translateY(-2px);
    box-shadow: 0 6px 20px rgba(0, 0, 0, 0.3);
}
</style>

<div class="error-container">
    <h1 class="error-code">404</h1>
    <h2 class="error-title">페이지를 찾을 수 없습니다</h2>
    <p class="error-title">Page Not Found</p>
    <p class="error-description">
        요청하신 페이지가 존재하지 않습니다.<br>
        The requested page does not exist.
    </p>
    <a href="<?= buildUrl('list') ?>" class="btn-home">
        ← 홈으로 돌아가기 (Back to Home)
    </a>
</div>

<?php
include LAYOUT_PATH . '/footer.php';
?>
