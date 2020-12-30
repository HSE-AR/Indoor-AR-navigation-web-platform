module.exports = {
    devServer: {
      open: process.platform === 'darwin',
      host: '0.0.0.0',
      port: 5000, // CHANGE YOUR PORT HERE!
      public: '192.168.0.103:5000',
      https: true,
      hotOnly: false,
    },
  }