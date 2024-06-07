module.exports = async () => {
    await import('@angular/compiler');

    return {
        ... await import('./httpclient-factory.mjs'),
        ... await import('authorize-client')
    };
};