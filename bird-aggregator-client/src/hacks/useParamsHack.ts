import { useParams } from 'react-router-dom'

// well, fucking shit https://github.com/remix-run/react-router/issues/8200
export function useParamsTyped<K extends string, V extends string | undefined = string | undefined>(): {[key in K]: V} {
	return useParams<K>() as {[key in K]: V}
}
