'use strict'

const noYieldWithoutStar = {
	meta: {
		type: 'problem',

		docs: {
			description: 'disallow using yield without *',
			category: 'Custom',
			recommended: false,
			suggestion: true,
		},

		fixable: 'code',

		schema: [],
		messages: {
			missing: 'Missing * after yield.',
		},
	},

	create(context) {
		const sourceCode = context.getSourceCode()

		function checkExpression(node) {
			const tokens = sourceCode.getFirstTokens(node, 2)
			const yieldToken = tokens[0]
			const starToken = tokens[1]

			if (starToken.value !== '*') {
				context.report({
					node,
					messageId: 'missing',
					fix(fixer) {
						return fixer.insertTextAfter(yieldToken, '*')
					},
				})
			}
		}

		return {
			YieldExpression: checkExpression,
		}
	},
}

const noOnClick = {
	meta: {
		type: 'problem',

		docs: {
			description: 'disallow using onClick event handler and prefer the use of onMouseDown',
			category: 'Possible Errors',
			recommended: false,
			suggestion: true,
		},

		fixable: 'code',

		schema: [],
		messages: {
			useOnMouseDownMessage: 'Do not use onClick event handler. Instead, use onMouseDown.',
		},
	},

	create(context) {
		function checkOnClickHandler(node) {
			if (node.name !== 'onClick') {
				return
			}

			context.report({
				node,
				messageId: 'useOnMouseDownMessage',
				fix: function (fixer) {
					return fixer.replaceText(node, 'onMouseDown')
				},
			})
		}

		return {
			JSXIdentifier: checkOnClickHandler,
			Identifier: checkOnClickHandler,
		}
	},
}

module.exports = {
	'no-yield-without-star': noYieldWithoutStar,
	'no-onclick': noOnClick,
}
